using RSI.Models;
using System.Collections.Generic;

namespace RSI.Services
{

    public interface IBorsaItalianaService
    {
        List<Quota> GetDailyQuotesLastMonth(string ticker);
        List<Quota> GetDailyQuotesLastThreeMonths(string ticker);
    }
}
