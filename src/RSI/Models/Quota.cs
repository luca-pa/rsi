using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSI.Models
{
    public class Quota
    {
        public string Ticker { get; set; }
        public DateTime Data { get; set; }
        public decimal Chiusura { get; set; }
        public long Volumi { get; set; }
        public string Variazione { get; set; }
        public decimal ChiusuraPrecedente { get; set; }

        public Etf Etf { get; set; }
    }
}
