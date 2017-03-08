using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImportYahooFinance
{
    public class Quota
    {
        public string Ticker { get; set; }
        public DateTime Data { get; set; }
        public decimal Chiusura { get; set; }
        public long Volumi { get; set; }
    }
}
