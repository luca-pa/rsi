using RSI.Models;
using System.Collections.Generic;

namespace RSI.Services
{
    public interface IYahooService
    {
        List<Quota> GetCurrentQuotes(IEnumerable<string> tickers);
        List<Quota> GetHistory(string ticker);
    }
}