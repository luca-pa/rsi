using RSI.Models;
using System.Linq;

namespace RSI.Repositories
{
    public interface IPortfolioRepository
    {
        Portafoglio GetPortafoglio();
        void AddPortafoglioItem(PortafoglioItem item);
        IQueryable<PortafoglioItem> GetPortafoglioItems(string ticker);
        void AggiornaBilancio(Bilancio bilancio);
        void Save();
    }
}