using System.Collections.Generic;
using RSI.Models;

namespace RSI.Services
{
    public interface IPortfolioService
    {
        Portafoglio GetPortafoglio();
        void UpdatePortafoglioItem(PortafoglioItem item);
        void AddPortafoglioItem(PortafoglioItem item);
        void AggiornaBilancio(Bilancio bilancio);
        int AggiornaQuotePortfolio();
        IEnumerable<StoricoItem> GetStoricoPerformance();
    }
}