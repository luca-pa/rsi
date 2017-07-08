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
            var items = _tradingContext.PortafoglioItems
                .Where(i => i.DataVendita.HasValue == false)
                .Include(p => p.Etf)
                .ToList();

            items.ForEach(i =>
            {
                var quota = _tradingContext.QuotePortafoglio.Where(q => q.Ticker == i.Ticker).OrderBy(q => q.Data).LastOrDefault();
                i.UltimaChiusura = quota?.Chiusura;
            });

            return new Portafoglio
            {
                Items = items,
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

        public IEnumerable<StoricoItem> GetPerformance()
        {
            return _tradingContext.StoricoItems.FromSql(PerformanceQuery).ToList();
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
            if (!quote.Any())
            {
                return 0;
            }

            var quoteEsistenti = _tradingContext.QuotePortafoglio.Where(q => q.Ticker == ticker).ToList();
            foreach (var quota in quote)
            {
                if (!quoteEsistenti.Any(q => q.Ticker == quota.Ticker && q.Data == quota.Data))
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

        const string PerformanceQuery =
            @"select a.Data, (valore-(Invested-Cash))/(Invested-Cash)*100 [Value]
            from (

	            select Data, sum(valore) [valore]
	            from (
		            select Data, (valore - tasse - commissione) [valore]
		            from (
			            select Data, valore, iif(valore > prezzo and etn = 0, (valore-prezzo)*26/100, 0) tasse, 5 commissione
			            from (
				            select qp.Data, Chiusura*Quantita valore, p.Prezzo*Quantita prezzo, Etn
				            from QuotePortafoglio qp 
				            join Portafoglio p on qp.Ticker = p.Ticker
				            join ETFs e on e.Ticker = p.Ticker
				            where (qp.Data between p.Data and dateadd(DAY, -1, p.DataVendita))
				            or (p.DataVendita is null and qp.Data >= p.Data)
			            ) t3
		            ) t2
	            ) t1
	            group by Data

            ) a join Bilancio b on b.Data = (select max(Data) from Bilancio where Data <= a.Data)
            order by a.Data";
    }
}
