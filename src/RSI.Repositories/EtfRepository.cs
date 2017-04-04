using RSI.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RSI.Repositories
{
    public class EtfRepository : IEtfRepository
    {
        private readonly TradingContext _tradingContext;

        public EtfRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public IEnumerable<Etf> GetSelezione()
        {
            return _tradingContext.Selezione
                 .Select(s => new Etf
                 {
                     Ticker = s.Ticker,
                     Isin = s.Etf.Isin,
                     Quote = s.Etf.Quote.Where(q => q.Ticker == s.Ticker).ToList()
                 });
        }

        public int Add(Quota quota)
        {
            _tradingContext.Quote.Add(quota);
            return _tradingContext.SaveChanges();
        }

        public int Update(Quota quotaEsistente)
        {
            _tradingContext.Update(quotaEsistente);
            return _tradingContext.SaveChanges();

        }

        int IEtfRepository.AggiornaQuoteMeseSuccessivo()
        {
            var meseCorrente = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var meseProssimo = meseCorrente.AddMonths(1).SetFirstWorkingDay();

            if (DateTime.Now < DateTime.Now.SetFirstWorkingDay())
            {
                meseProssimo = meseCorrente.SetFirstWorkingDay();
                meseCorrente = meseCorrente.AddMonths(-1);
            }

            var quoteToAddOrUpdate = _tradingContext.Quote
                .Where(q => q.Data.ToString("MMyyyy") == meseCorrente.ToString("MMyyyy"))
                .Select(q => new Quota
                {
                    Ticker = q.Ticker,
                    Data = meseProssimo,
                    Chiusura = q.Chiusura,
                    Volumi = q.Volumi
                })
                .ToList();

            int counter = 0;
            var quoteEsistenti = _tradingContext.Quote.Where(q => q.Data == meseProssimo).Select(q => new { q.Ticker, q.Chiusura }).ToList();

            foreach (var quota in quoteToAddOrUpdate)
            {
                var quotaEsistente = quoteEsistenti.FirstOrDefault(q => q.Ticker == quota.Ticker);
                if (quotaEsistente != null)
                {
                    if (quotaEsistente.Chiusura != quota.Chiusura)
                    {
                        counter += Update(quota);
                    }
                }
                else
                {
                    counter += Add(quota);
                }
            }

            return counter;
        }

        public int AggiornaQuotePortfolio()
        {
            throw new NotImplementedException();
        }
    }
}
