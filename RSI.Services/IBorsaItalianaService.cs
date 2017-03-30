using RSI.Models;
using System.Collections.Generic;

namespace RSI.Services
{

    public interface IBorsaItalianaService
    {
        List<Quota> GetDailyQuotes(string ticker, string range);
    }
}
