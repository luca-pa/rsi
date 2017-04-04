using RSI.Models;
using System;
using System.Collections.Generic;

namespace RSI.Services
{
    public interface IYahooService
    {
        List<Quota> GetCurrentQuotes(IEnumerable<string> tickers);
        List<QuotaPortafoglio> GetQuotes(string ticker, DateTime startDate, DateTime endDate);
    }
}