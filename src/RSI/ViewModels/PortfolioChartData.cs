using System;
using System.Collections.Generic;
using RSI.Models;
using System.Linq;

namespace RSI.ViewModels
{
    public class PortfolioChartData
    {
        public IEnumerable<decimal> Values => orderedItems.Select(i => Math.Round(i.Value, 2));
        public IEnumerable<string> Times => orderedItems.Select(i => i.Data.ToString("yyyy-MM-dd"));

        private readonly IOrderedEnumerable<StoricoItem> orderedItems;

        public PortfolioChartData(IEnumerable<StoricoItem> items)
        {
            orderedItems = items.OrderBy(i => i.Data);
        }
    }
}
