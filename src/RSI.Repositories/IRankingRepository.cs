using RSI.Models;
using System;
using System.Collections.Generic;

namespace RSI.Repositories
{
    public interface IRankingRepository
    {
        IEnumerable<RankedEtf> GetSelezione(DateTime? dataRiferimento);
        IEnumerable<RankedEtf> GetAll(DateTime? dataRiferimento, bool getShorts, bool getDistribution, bool onlyEtcs);
        void RemoveFromSelection(string ticker);
        void AddToSelection(string ticker);
    }
}
