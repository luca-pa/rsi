using RSI.Models;
using RSI.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RSI.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IYahooService _yahooService;
        private readonly ITraderLinkService _traderLinkService;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IBorsaItalianaService _borsaItalianaService;

        public PortfolioService(IBorsaItalianaService borsaItalianaService, IYahooService yahooService, ITraderLinkService traderLinkService, IPortfolioRepository portfolioRepository)
        {
            _borsaItalianaService = borsaItalianaService;
            _traderLinkService = traderLinkService;
            _yahooService = yahooService;
            _portfolioRepository = portfolioRepository;
        }

        public Portafoglio GetPortafoglio()
        {
            var portafoglio = _portfolioRepository.GetPortafoglio();
            var quote = _yahooService.GetCurrentQuotes(portafoglio.Items.Select(i => i.Ticker));
            var quotePrecedenti = quote.Select(q => new { q.Ticker, q.ChiusuraPrecedente });

            if (_traderLinkService.IsServiceAvailable())
            {
                quote = _traderLinkService.GetCurrentQuotes(portafoglio.Items.Select(i => i.Etf).ToList());
            }

            portafoglio.Items.ForEach(item =>
                SetCurrentPrice(
                    item,
                    quote.SingleOrDefault(q => q.Ticker == item.Ticker),
                    quotePrecedenti.SingleOrDefault(q => q.Ticker == item.Ticker)?.ChiusuraPrecedente
                )
            );

            return portafoglio;
        }

        public IEnumerable<StoricoItem> GetStoricoPerformance()
        {
            return _portfolioRepository.GetPerformance();
        }

        public void AddPortafoglioItem(PortafoglioItem item)
        {
            _portfolioRepository.AddPortafoglioItem(item);
        }

        public void UpdatePortafoglioItem(PortafoglioItem item)
        {
            var itemToUpdate = _portfolioRepository.GetPortafoglioItems(item.Ticker).FirstOrDefault(i => i.DataVendita == null);
            if (itemToUpdate != null)
            {
                if (item.DataVendita.HasValue && item.DataVendita > itemToUpdate.Data &&
                    item.PrezzoVendita.HasValue && item.PrezzoVendita.Value > 0)
                {
                    itemToUpdate.DataVendita = item.DataVendita;
                    itemToUpdate.PrezzoVendita = item.PrezzoVendita;
                }
                itemToUpdate.Dividendi = item.Dividendi;

                _portfolioRepository.Save();
            }

        }

        public void AggiornaBilancio(Bilancio bilancio)
        {
            if (bilancio != null)
            {
                _portfolioRepository.AggiornaBilancio(bilancio);
            }
        }

        public int AggiornaQuotePortfolio()
        {
            int counter = 0;

            _portfolioRepository.GetAllPortafoglioItems()
                .ToList()
                .ForEach(p =>
                {
                    if (p.DataVendita == null || p.DataVendita > DateTime.Now.AddMonths(-1)) {
                        var quote = _borsaItalianaService.GetDailyQuotes(p.Ticker, "1m")
                                        .Where(q => q.Data >= p.Data && (p.DataVendita == null || q.Data <= p.DataVendita))
                                        .Select(q => new QuotaPortafoglio { Ticker = q.Ticker, Data = q.Data, Chiusura = q.Chiusura })
                                        .ToList();
                        counter += _portfolioRepository.AggiornaQuotePortafoglio(p.Ticker, quote);
                    }
                });

            return counter;
        }


        void SetCurrentPrice(PortafoglioItem item, Quota quota, decimal? chiusuraPrecedente)
        {
            if (quota == null)
                return;

            item.PrezzoCorrente = quota.Chiusura;
            if (string.IsNullOrEmpty(quota.Variazione))
            {
                if (chiusuraPrecedente.HasValue)
                {
                    item.Variazione = ((quota.Chiusura - chiusuraPrecedente) / chiusuraPrecedente).Value.ToString("P2");
                }
            }
            else
            {
                item.Variazione = quota.Variazione;
            }
        }
    }

}
