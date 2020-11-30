using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QLess.Models;
using System.IO;
using System.Text;

namespace QLess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QLessController : ControllerBase
    {
        private readonly ILogger<QLessController> _logger;
       // private static QLessModel[] QLessItems = { };
        private static List<QLessModel> QLessModelsItems = new List<QLessModel>();
        private static List<QLessRegistration> QLessRegistrationItems = new List<QLessRegistration>();
        private static PriceMatrix[] PriceMatrixLine1Items = { };
        private static PriceMatrix[] PriceMatrixLine2Items = { };
        static string dir = @"c:\Users";
        static string qlessfilePath = @$"{dir}\qlessdata.txt";
        static string qlessRegfilePath = @$"{dir}\qlessRegistrationData.txt";
        static string priceMatrixfilePath = @$"{dir}\priceMatrixData{{0}}.txt";
        public PriceMatrix priceMatrix;
        public QLessController(ILogger<QLessController> logger)
        {
            _logger = logger;
            //DataUtil.createQLessJSON(null);
            //DataUtil.createQLessRegistrationJSON(null);
            //DataUtil.createPriceMatrixJSON();

            QLessModelsItems = DataUtil.OpenQLessJsonData().ToList();
            QLessRegistrationItems = DataUtil.OpenQLessRegistrationJsonData();
            PriceMatrixLine1Items = DataUtil.OpenPriceJsonData().Item1;
            PriceMatrixLine2Items = DataUtil.OpenPriceJsonData().Item2;
        }

        

        [HttpGet]
        public QLessViewModel Get(int id, string stnFrom, string line)
        {
           
            QLessViewModel viewModel = new QLessViewModel();
            var res = QLessModelsItems.Where(q => q.Id == id).FirstOrDefault();
            var errMsg = Validate(res);
            if (string.IsNullOrEmpty(errMsg))
            {
                ProcessResult(res, stnFrom, line);
                viewModel.QLessModel = res;
                viewModel.PriceMatrix = priceMatrix;
                viewModel.IsValid = true;
            } else
            {
                viewModel.QLessModel = res;
                viewModel.Status = errMsg;
                viewModel.PriceMatrix = new PriceMatrix();
                viewModel.IsValid = false;
            }

            return viewModel;
        }


        public string Validate(QLessModel res)
        {
            var errMsg = "";
         
            if (DateHelper.CardAge(res.DateLastUsed) > 5)
            {
                errMsg = "Card has expired!";
            }
            return errMsg;
        }

        [HttpPost]
        public QLessViewModel Register(QLessRegistration qLessRegistration)
        {
            QLessViewModel viewModel = new QLessViewModel();
            try
            {
               
                var res = QLessModelsItems.Where(q => q.Id == qLessRegistration.QLessCardId).FirstOrDefault();
                viewModel.QLessModel = res;
                var maxId = (QLessRegistrationItems !=null && QLessRegistrationItems.Any()) ? QLessRegistrationItems.Max(c => c.Id) + 1 : 1;
                QLessRegistration reg = new QLessRegistration()
                {
                    SrCCN = qLessRegistration.SrCCN,
                    QLessCardSerialNo = res.SerialNo,
                    PwdId = qLessRegistration.PwdId,
                    Id = maxId
                };

                QLessRegistrationItems.Add(reg);
                DataUtil.createQLessRegistrationJSON(QLessRegistrationItems.ToArray());
                viewModel.Status = "success";
                return viewModel;
            }
            catch(Exception x) {
                viewModel.Status = "fail";
                return viewModel;
            }
            
           
        }
        /// <summary>
        /// Calculate and Save Card load and other properties only upon arrival at destination
        /// function that will supposedly save into db after card scanned on arrival to destination but instead I'm saving it to a JSON file on scan for shorter version
        /// </summary>
        /// <param name="res"></param>
        private void ProcessResult(QLessModel res, string stnFrom, string line)
        {
            var isRegistered = QLessRegistrationItems.Any(r => r.QLessCardSerialNo == res.SerialNo);

            res.Type = isRegistered ? CardType.Discounted : CardType.Regular;
            res.IsEntry = !string.IsNullOrEmpty(stnFrom) ? false : true;
            var cardInfo = GetValueByFromToStn(res, stnFrom, line);
            res.Value = cardInfo.Item1;
            if (!res.IsEntry)
            {
                if (res.DateLastUsed.Date != DateTime.Today) //reset to 0 for the day
                {
                    res.noOfUseToday = 0;
                }
                res.noOfUseToday++;
                res.Value = cardInfo.Item2;
                QLessModelsItems = QLessModelsItems.Where(c => c.Id != res.Id).ToList();
                QLessModelsItems.Add(res);
                DataUtil.createQLessJSON(QLessModelsItems.ToArray()); //supposedly save changes in db using data layer
            }

        }

        /// <summary>
        /// Get card balance, total balance after fare deduction, if stnfrom is existing means customer is on exit mode
        /// </summary>
        /// <param name="qLess"></param>
        /// <returns>Item1 - card balance
        /// Item2 - total = bal - fare
        /// </returns>
        private Tuple<double, double> GetValueByFromToStn(QLessModel qLess, string stnFrom, string lineArg)
        {
            priceMatrix = new PriceMatrix();
            var noOfUseMaxedDiscnted = qLess.noOfUseToday <= 4 && qLess.Type == CardType.Discounted;
            var noOfUseMaxed3PcntDisc = (qLess.noOfUseToday <= 4 && qLess.noOfUseToday > 1) && qLess.Type == CardType.Discounted;

            Random random = new Random();
            int line = !string.IsNullOrEmpty(lineArg) ? Convert.ToInt32(lineArg) : random.Next(1,3); //MRT line 1 , MRT line 2
            int entryStn = !string.IsNullOrEmpty(stnFrom) ? Convert.ToInt32(stnFrom) : random.Next(line == 1 ? 11 : 20);
            int exitStn = random.Next(line == 1 ? 11 : 20); //line 1 has 1 stns, line 2 20 stns
            var regFare = GetPriceMatrix(entryStn, exitStn, line);
            var farePriceByType = noOfUseMaxedDiscnted ? (regFare - (regFare * .20)) : regFare;

            priceMatrix.Line = line;
            priceMatrix.StationFr = entryStn;
            priceMatrix.StationTo = exitStn;
            priceMatrix.StationFrName = Enum.GetName(typeof(Line1Stations), entryStn);
            priceMatrix.StationToName = Enum.GetName(typeof(Line1Stations), exitStn);
            priceMatrix.Price = regFare;
            var dicntedFare = noOfUseMaxed3PcntDisc ? (farePriceByType - (farePriceByType * .03)) : farePriceByType;
            qLess.Discount = regFare - dicntedFare;
            return Tuple.Create(qLess.Value, qLess.Value - dicntedFare);
        }

        
        /// <summary>
        /// Random Price Matrix , Appendix A  - Price Metrix not followed exact fare - too long to code
        /// </summary>
        /// <param name="stnEntry"></param>
        /// <param name="stnExit"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static double GetPriceMatrix(int stnEntry, int stnExit, int line)
        {
            var priceList = line == 1 ? PriceMatrixLine1Items : PriceMatrixLine2Items;
            var fare = priceList
                .Where(p => p.StationFr == stnEntry && p.StationTo == stnExit)
                .Select(p=> p.Price).FirstOrDefault();
            return fare;
        }
    }
    
}