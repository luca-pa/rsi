using System;
using System.Collections.Generic;

namespace RSI.Models
{
    public class Ranking
    {
        public DateTime DataRiferimento { get; set; }
        public List<RankedEtf> Etfs { get; set; }
    }
}
