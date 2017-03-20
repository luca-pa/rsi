using Microsoft.EntityFrameworkCore;
using RSI.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace RSI.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly TradingContext _tradingContext;

        public PortfolioRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public Portafoglio GetPortafoglio()
        {
            return new Portafoglio
            {
                Items = _tradingContext.PortafoglioItems.Where(i => i.DataVendita.HasValue == false).Include(p => p.Etf).ToList(),
                Bilancio = _tradingContext.Bilancio.OrderByDescending(b => b.Data).FirstOrDefault()
            };
        }

        public IQueryable<PortafoglioItem> GetPortafoglioItems(string ticker)
        {
            return _tradingContext.PortafoglioItems.Where(i => i.Ticker == ticker);
        }

        public IQueryable<PortafoglioItem> GetAllPortafoglioItems()
        {
            return _tradingContext.PortafoglioItems;
        }

        public void AddPortafoglioItem(PortafoglioItem item)
        {
            if (_tradingContext.Etfs.Any(e => e.Ticker == item.Ticker))
            {
                _tradingContext.PortafoglioItems.Add(item);
                Save();
            }
        }

        public void AggiornaBilancio(Bilancio bilancio)
        {
            if (_tradingContext.Bilancio.Any(b => b.Data == bilancio.Data))
            {
                _tradingContext.Update(bilancio);
            }
            else
            {
                _tradingContext.Bilancio.Add(bilancio);
            }
            Save();
        }

        public int AggiornaQuotePortafoglio(string ticker, List<QuotaPortafoglio> quote)
        {
            var quoteEsistenti = _tradingContext.QuotePortafoglio.Where(q => q.Ticker == ticker).ToList();
            foreach(var quota in quote)
            {
                if(!quoteEsistenti.Any(q=>q.Ticker==quota.Ticker && q.Data==quota.Data))
                {
                    _tradingContext.QuotePortafoglio.Add(quota);
                }
            }
            return _tradingContext.SaveChanges();
        }

        public void Save()
        {
            _tradingContext.SaveChanges();
        }
    }
}
