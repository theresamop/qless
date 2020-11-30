using Newtonsoft.Json;
using QLess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLess
{
    public static class DataUtil
    {
        static string dir = @"c:\Users";
        static string qlessfilePath = @$"{dir}\qlessdata.txt";
        static string qlessRegfilePath = @$"{dir}\qlessRegistrationData.txt";
        static string priceMatrixfilePath = @$"{dir}\priceMatrixData{{0}}.txt";
        private static QLessModel[] QLessItems = { };
        private static QLessRegistration[] QLessRegistrationItems = { };
        private static PriceMatrix[] PriceMatrixLine1Items = { };
        private static PriceMatrix[] PriceMatrixLine2Items = { };
        public static void createQLessJSON(QLessModel[] qLessModels)
        {
            var QLessItems1 = new QLessModel[]
               {
                   new QLessModel() { Id = 1, DateLastUsed = DateTime.Today, Value = 100, noOfUseToday = 1, Type = CardType.Discounted, PurchaseDate = DateTime.Now, SerialNo="1A"},
                   new QLessModel() { Id = 2, DateLastUsed = DateTime.Today.AddDays(-30), Value = 100,  noOfUseToday = 5 , Type = CardType.Regular, PurchaseDate = DateTime.Now,  SerialNo="2B"},
                   new QLessModel() { Id = 3, DateLastUsed = DateTime.Today.AddYears(-5), Value = 100, noOfUseToday = 1, Type = CardType.Discounted, PurchaseDate = DateTime.Now, SerialNo="3C"},
                   new QLessModel() { Id = 4, DateLastUsed = DateTime.Today.AddYears(-6), Value = 100,  noOfUseToday = 3, Type = CardType.Discounted, PurchaseDate = DateTime.Now, SerialNo="4D"},
                   new QLessModel() { Id = 5, DateLastUsed = DateTime.Today, Value = 100, noOfUseToday = 1, Type = CardType.Regular, PurchaseDate = DateTime.Now, SerialNo="5E"},
               };
            using (StreamWriter sw = System.IO.File.CreateText(qlessfilePath))
            {
                if (qLessModels!= null && qLessModels.Any())
                {
                    sw.WriteLine(JsonConvert.SerializeObject(qLessModels));
                }
                else
                {
                    sw.WriteLine(JsonConvert.SerializeObject(QLessItems1));
                }
               
            }

        }
        public static void createQLessRegistrationJSON(QLessRegistration[] qLessRegistrationItems)
        {
            var QLessRegistrationItems1 = new QLessRegistration[]
              {
                  
                   new QLessRegistration() { Id = 1, PwdId = "1234567891233", QLessCardId = 1, QLessCardSerialNo = "1A", SrCCN = "1223456789"  },
                   new QLessRegistration() { Id = 1, PwdId = "1234567891234", QLessCardId = 1, QLessCardSerialNo = "2B", SrCCN = "1223456789"  },
                   new QLessRegistration() { Id = 1, PwdId = "3333333333", QLessCardId = 1, QLessCardSerialNo = "3C", SrCCN = "6666666"  },
              };
            using (StreamWriter sw = System.IO.File.CreateText(qlessRegfilePath))
            {
                if (qLessRegistrationItems != null && qLessRegistrationItems.Any())
                {
                    sw.WriteLine(JsonConvert.SerializeObject(qLessRegistrationItems));
                }
                else
                {
                    sw.WriteLine(JsonConvert.SerializeObject(QLessRegistrationItems1));
                }

            }

        }
        public static void createPriceMatrixJSON()
        {
           
            var minFare = 11;

            for (int line = 1; line <= 2; line++)
            {
                var prices = new List<PriceMatrix>();
                StringBuilder sb = new StringBuilder();
             
                var lineStns = 11;
                if (line == 2)
                {
                    lineStns = 20;
                }
                for (int i = 0; i <= lineStns; i++)
                {


                    for (int j = 0; j <= lineStns; j++)
                    {
                        PriceMatrix pm = new PriceMatrix();
                        pm.Line = line;
                        pm.StationFr = i;
                        pm.StationTo = j;
                        if (i == j)
                        {

                            pm.Price = minFare;
                        }
                        else if (i == 0 && j > 0)
                        {
                            pm.Price = minFare + j;
                        }
                        else if (j == 0 && i > 0)
                        {
                            pm.Price = minFare + i;
                        }
                        else
                        {
                            pm.Price = minFare + (i + j);
                        }

                        sb.Append(pm.Price.ToString().PadRight(10));
                        prices.Add(pm);
                    }
                    sb.Append(Environment.NewLine);

                }
                using (StreamWriter sw = System.IO.File.CreateText(string.Format(priceMatrixfilePath, line)))
                {
                    sb.ToString();
                    sw.WriteLine(JsonConvert.SerializeObject(prices));
                }
            }

        }
        public static QLessModel[] OpenQLessJsonData()
        {
            string jsonData = System.IO.File.ReadAllText(qlessfilePath);
            QLessItems = JsonConvert.DeserializeObject<QLessModel[]>(jsonData);

            return QLessItems;
        }

        public static List<QLessRegistration> OpenQLessRegistrationJsonData()
        {
            string jsonData = System.IO.File.ReadAllText(qlessRegfilePath);
            QLessRegistrationItems = JsonConvert.DeserializeObject<QLessRegistration[]>(jsonData);

            return QLessRegistrationItems.ToList();
        }
        public static Tuple<PriceMatrix[], PriceMatrix[]> OpenPriceJsonData()
        {
            for (int i = 1; i <= 2; i++)
            {
                string jsonData = System.IO.File.ReadAllText(string.Format(priceMatrixfilePath, i));
                var results = JsonConvert.DeserializeObject<PriceMatrix[]>(jsonData);
                if (i == 1)
                {
                    PriceMatrixLine1Items = results;
                }
                else
                {
                    PriceMatrixLine2Items = results;
                }

            }

            return Tuple.Create(PriceMatrixLine1Items, PriceMatrixLine2Items);
        }
       
    }
}
