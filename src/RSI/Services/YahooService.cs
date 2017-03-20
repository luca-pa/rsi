using RSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RSI.Services
{
    public class YahooService : IYahooService
    {
        public List<Quota> GetCurrentQuotes(IEnumerable<string> tickers)
        {
            var tickersString = string.Join(".MI+", tickers);
            string url = $"http://download.finance.yahoo.com/d/quotes.csv?s={tickersString}.MI&f=sl1p2p&e=.csv";
            bool marketOpen = false;

            if (DateTime.Now.TimeOfDay > new TimeSpan(9, 20, 0) &&
                DateTime.Now.TimeOfDay < new TimeSpan(17, 52, 0) &&
                !(new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek))
                )
            {
                marketOpen = true;
                url = url.Replace("f=sl1p2p", "f=sbk2p");
            }
             

            var quote = new List<Quota>();

            var csvData = GetData(url);
            if (csvData != null)
            {
                var lines = csvData.Split('\n');
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Count() > 1)
                    {
                        var ticker = values[0].Replace("\"", "").Replace(".MI", "");
                        var prezzo = decimal.Parse(values[1].Replace(".", ","));
                        var variazione = values[2].Replace("\"", "").Replace(".", ",");
                        var chiusuraPrecedente = decimal.Parse(values[3].Replace(".", ","));
                        if (marketOpen)
                        {
                            variazione = ((prezzo - chiusuraPrecedente) / chiusuraPrecedente).ToString("P2");
                        }
                        quote.Add(new Quota
                        {
                            Ticker = ticker,
                            Chiusura = prezzo,
                            Variazione = variazione,
                            ChiusuraPrecedente = chiusuraPrecedente
                        });
                    }
                }
            }
            return quote;
        }

        public List<QuotaPortafoglio> GetQuotes(string ticker, DateTime startDate, DateTime endDate)
        {
            string url = $"http://real-chart.finance.yahoo.com/table.csv?s={ticker}.MI";
            url += $"&a={startDate.Month - 1}&b={startDate.Day}&c={startDate.Year}&d={endDate.Month - 1}&e={endDate.Day}&f={endDate.Year}&g=d&ignore=.csv";

            var quote = new List<QuotaPortafoglio>();

            var csvData = GetData(url);
            if (csvData != null)
            {
                var lines = csvData.Split('\n');
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    DateTime data;

                    if (!DateTime.TryParse(values[0], out data))
                        continue;

                    var chiusura = decimal.Parse(values[6].Replace(".", ","));

                    quote.Add(new QuotaPortafoglio
                    {
                        Ticker = ticker.Trim(),
                        Data = data,
                        Chiusura = chiusura
                    });

                }
            }
            return quote;
        }


        private static string GetData(string url)
        {
            var task = GetDataAsync(url);
            task.Wait();

            return task.Result;
        }

        private static async Task<string> GetDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();

            }
        }

    }
}
