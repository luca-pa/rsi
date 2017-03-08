using RSI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSI.Services
{
    public class EtfService : IEtfService
    {
        private readonly ITraderLinkService _traderLinkService;
        private readonly IEtfRepository _etfRepository;

        public EtfService(ITraderLinkService traderLinkService, IEtfRepository etfRepository)
        {
            _traderLinkService = traderLinkService;
            _etfRepository = etfRepository;
        }

        public int AggiornaQuoteSelezione()
        {

            var etfs = _etfRepository.GetSelezione().ToList();

            int recordsUpdated = 0;

            var quote = _traderLinkService.GetCurrentQuotes(etfs);

            foreach (var etf in etfs)
            {
                var quota = quote.FirstOrDefault(q => q.Ticker == etf.Ticker);

                if (quota != null)
                {
                    var changes = 0;
                    var quotaEsistente = etf.Quote.SingleOrDefault(q => q.Data.ToString("MMyyyy") == quota.Data.ToString("MMyyyy"));
                    if (quotaEsistente == null)
                    {
                        changes = _etfRepository.Add(quota);
                    }
                    else if (quota.Chiusura > 0 && quota.Chiusura != quotaEsistente.Chiusura)
                    {
                        quotaEsistente.Chiusura = quota.Chiusura;
                        if (quota.Volumi != -1)
                        {
                            quotaEsistente.Volumi = quota.Volumi;
                        }
                        changes = _etfRepository.Update(quotaEsistente);
                    }
                    recordsUpdated += changes;
                }
            }
            return recordsUpdated;
        }

        public int AggiornaQuoteMeseSuccessivo()
        {
            return _etfRepository.AggiornaQuoteMeseSuccessivo();
        }
    }
}
