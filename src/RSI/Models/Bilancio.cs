using System;

namespace RSI.Models
{
    public class Bilancio
    {
        public DateTime Data { get; set; }
        public decimal Invested { get; set; }
        public decimal Cash { get; set; }
        public decimal Minusvalenze { get; set; }
    }
}
