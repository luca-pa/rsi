using RSI.Models;
using RSI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSI.Services
{
    public class RankingService : IRankingService
    {
        private readonly IRankingRepository _rankingRepository;

        public RankingService(IRankingRepository rankingRepository)
        {
            _rankingRepository = rankingRepository;
        }

        public Ranking GetSelezione(DateTime? dataRiferimento = null)
        {
            dataRiferimento = dataRiferimento ?? DateTime.Now;

            var etfs = _rankingRepository.GetSelezione(dataRiferimento);

            return new Ranking
            {
                DataRiferimento = dataRiferimento.Value,
                Etfs = etfs.ToList()
            };
        }

        public Ranking GetAll(DateTime? dataRiferimento, bool getShorts, bool getDistributions, int minVolumes)
        {
            return GetAllInternal(_rankingRepository.GetAll, ref dataRiferimento, getShorts, getDistributions, minVolumes, onlyEtcs: false);
        }

        public Ranking GetAllEtcs(DateTime? dataRiferimento, bool getShorts, bool getDistributions, int minVolumes)
        {
            return GetAllInternal(_rankingRepository.GetAll, ref dataRiferimento, getShorts, getDistributions, minVolumes, onlyEtcs: true);
        }

        private Ranking GetAllInternal(Func<DateTime?, bool, bool, bool, IEnumerable<RankedEtf>> func, ref DateTime? dataRiferimento, bool getShorts, bool getDistributions, int minVolumes, bool onlyEtcs)
        {
            dataRiferimento = dataRiferimento ?? DateTime.Now;

            var etfs = func(dataRiferimento, getShorts, getDistributions, onlyEtcs);

            return new Ranking
            {
                DataRiferimento = dataRiferimento.Value,
                Etfs = etfs.Where(e => e.Quote.Any(q => q.Data > DateTime.Now.AddMonths(-2)) && e.MediaVolumi10 > minVolumes).ToList()
            };
        }

        public void AddToSelection(string ticker)
        {
            _rankingRepository.AddToSelection(ticker);
        }

        public void RemoveFromSelection(string ticker)
        {
            _rankingRepository.RemoveFromSelection(ticker);
        }
    }

}
