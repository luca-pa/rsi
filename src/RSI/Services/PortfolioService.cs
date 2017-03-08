using RSI.Models;
using RSI.Repositories;
using System.Linq;

namespace RSI.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IYahooService _yahooService;
        private readonly ITraderLinkService _traderLinkService;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IYahooService yahooService, ITraderLinkService traderLinkService, IPortfolioRepository portfolioRepository)
        {
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
