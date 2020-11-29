using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLess.Models
{
    public class QLessViewModel
    {
        public QLessModel QLessModel { get; set; }
        public PriceMatrix PriceMatrix { get; set; }

        public string Status { get; set; }
    }
}
