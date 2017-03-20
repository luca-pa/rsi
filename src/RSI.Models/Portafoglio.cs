using System;
using System.Collections.Generic;

namespace RSI.Models
{
    public class Portafoglio
    {
        public List<PortafoglioItem> Items { get; set; }
        public Bilancio Bilancio { get; set; }
    }
}
