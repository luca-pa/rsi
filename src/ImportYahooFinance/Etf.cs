using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportYahooFinance
{
    public class Etf
    {
        public string Ticker { get; set; }
        public string Nome { get; set; }
        public string Indice { get; set; }
        public string Emittente { get; set; }
        public bool Leveraged { get; set; }
        public bool Short { get; set; }
    }
}
