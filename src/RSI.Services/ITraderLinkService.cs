using RSI.Models;
using System.Collections.Generic;

namespace RSI.Services
{
    public interface ITraderLinkService
    {
        List<Quota> GetCurrentQuotes(IList<Etf> etfs);
        bool IsServiceAvailable();
    }
}