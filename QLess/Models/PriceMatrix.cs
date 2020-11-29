using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLess.Models
{
    public class PriceMatrix
    {
        public int StationFr {get; set;}
        public int StationTo { get; set; }
        public double Price { get; set; }

        public int Line { get; set; }

        public string StationFrName { get; set; }
        public string StationToName { get; set; }
    }

    public enum Line1Stations
    {
        Baclaran =0,
        Edsa =1,
        Libertad = 2,
        GPuyat = 3,
        VCruz = 4,
        Quirino = 5,
        PGil = 6,
        UN = 7,
        CTerminal = 8,
        Capredo = 9,
        DJose = 10,
        Bambang = 11,
        Tayuman = 12,
        Blumnentrit = 13,
        ASantos = 14,
        RPapa = 15,
        FthAve = 16,
        Monumento = 17,
        Balintawak = 18,
        Roosevelt = 19,
       
    }

    public enum Line2Stations
    {
        Recto = 0,
        Legarda = 1,
        Pureza = 2,
        VMapa = 3,
        JRuiz = 4,
        Gilmore = 5,
        BettyGO = 6,
        Cubao = 7,
        Anonas = 8,
        Katipunan = 9,
        Santolan = 10,
      

    }
}
