using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSI.Models
{
    public class QuotaPortafoglio
    {
        public string Ticker { get; set; }
        public DateTime Data { get; set; }
        public decimal Chiusura { get; set; }
    }
}
