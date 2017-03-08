using RSI.Models;
using System.Collections.Generic;

namespace RSI.Repositories
{
    public interface IEtfRepository
    {
        IEnumerable<Etf> GetSelezione();
        int Add(Quota quota);
        int Update(Quota quotaEsistente);
        int AggiornaQuoteMeseSuccessivo();
    }
}
