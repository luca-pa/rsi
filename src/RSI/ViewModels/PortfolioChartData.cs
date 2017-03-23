using System;
using System.Collections.Generic;
using RSI.Models;
using System.Linq;

namespace RSI.ViewModels
{
    public class PortfolioChartData
    {
        public string Data { get; set; }
        public decimal Value { get; set; }

        public PortfolioChartData(StoricoItem item)
        {
            Data = item.Data.ToString("yyyy-MM-dd");
            Value = Math.Round(item.Value, 2);
        }
    }
}
