using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLess.Models
{
    public class QLessModel
    {
        public long Id { get; set; }
        public double Value { get; set; }
        //public DateTime ExpirationDate { get; set; }
        public string Type { get; set; }
        public int noOfUseToday { get; set; }
        public DateTime DateLastUsed { get; set; }
        // public long ? RegisterId { get; set; }

        public DateTime PurchaseDate { get; set; }
        public Boolean IsEntry { get; set; }

        public string SerialNo { get; set; }

        public double Discount { get; set; }

    }
    public class QLessRegistration
    {
        public long Id { get; set; }
        public string SrCCN { get; set; }
        public string PwdId { get; set; }
        public string QLessCardSerialNo { get; set; }
        public long QLessCardId { get; set; }

    }
    public struct CardType
    {
        public const string Regular = "Regular";
        public const string Discounted = "Discounted";
    }
}
