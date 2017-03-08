using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RSI.Models
{
    public class Selezione
    {
        public string Ticker { get; set; }
        public Etf Etf { get; set; }
    }
}
