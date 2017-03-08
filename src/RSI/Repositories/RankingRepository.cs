using System;
using System.Collections.Generic;
using System.Linq;
using RSI.Models;
using Microsoft.EntityFrameworkCore;

namespace RSI.Repositories
{
    public class RankingRepository : IRankingRepository
    {
        private readonly TradingContext _tradingContext;

        public RankingRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public IEnumerable<RankedEtf> GetSelezione(DateTime? dataRiferimento)
        {
            return _tradingContext.Selezione
                .Select(s => new RankedEtf
                {
                    Ticker = s.Ticker,
                    Isin = s.Etf.Isin,
                    Nome = s.Etf.Nome,
                    Etn = s.Etf.Etn,
                    Quote = s.Etf.Quote.Where(q => q.Ticker == s.Ticker).ToList(),
                    IsOwned = PortafoglioTickers(dataRiferimento).Contains(s.Ticker),
                    DataRiferimento = dataRiferimento.Value
                });
        }

        public IEnumerable<RankedEtf> GetAll(DateTime? dataRiferimento, bool getShorts)
        {
            return _tradingContext.Etfs.Include(e => e.Quote)
                .Where(e => e.Distribuzione == false)
                .Where(e => e.Leveraged == false)
                .Where(e => getShorts || e.Short == getShorts)
                .Where(e => e.Quote.Count > 10)
                .Select(e => new RankedEtf
                {
                    Ticker = e.Ticker,
                    Isin = e.Isin,
                    Nome = e.Nome,
                    Etn = e.Etn,
                    Quote = e.Quote,
                    IsOwned = PortafoglioTickers(dataRiferimento).Contains(e.Ticker),
                    DataRiferimento = dataRiferimento.Value
                });
        }


        public void RemoveFromSelection(string ticker)
        {
            _tradingContext.Attach(new Selezione { Ticker = ticker }).State = EntityState.Deleted;
            _tradingContext.SaveChanges();
        }

        public void AddToSelection(string ticker)
        {
            _tradingContext.Selezione.Add(new Selezione { Ticker = ticker });
            _tradingContext.SaveChanges();

        }



        private List<string> PortafoglioTickers(DateTime? dataRiferimento)
        {
            return _tradingContext.PortafoglioItems
                .Where(i =>
                    (dataRiferimento > DateTime.Now && !i.DataVendita.HasValue) ||
                    (dataRiferimento < DateTime.Now && i.DataVendita.HasValue && dataRiferimento <= i.DataVendita && dataRiferimento > i.Data) ||
                    (dataRiferimento < DateTime.Now && !i.DataVendita.HasValue && dataRiferimento > i.Data)
                )
                .Select(p => p.Ticker.Trim())
                .ToList();
        }
    }
}
