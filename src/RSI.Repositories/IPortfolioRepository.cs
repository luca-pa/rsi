using RSI.Models;
using System.Linq;
using System.Collections.Generic;

namespace RSI.Repositories
{
    public interface IPortfolioRepository
    {
        Portafoglio GetPortafoglio();
        void AddPortafoglioItem(PortafoglioItem item);
        IQueryable<PortafoglioItem> GetPortafoglioItems(string ticker);
        IQueryable<PortafoglioItem> GetAllPortafoglioItems();
        void AggiornaBilancio(Bilancio bilancio);
        int AggiornaQuotePortafoglio(string ticker, List<QuotaPortafoglio> quote);
        void Save();
    }
}