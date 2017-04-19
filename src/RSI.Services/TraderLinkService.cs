using RSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RSI.Services
{
    public class TraderLinkService : ITraderLinkService
    {
        bool IsMarketOpen => (
            DateTime.Now.TimeOfDay > new TimeSpan(9, 0, 0) &&
            DateTime.Now.TimeOfDay < new TimeSpan(17, 36, 0) &&
            !IsWeekend
            );

        private bool IsWeekend => new[] { DayOfWeek.Saturday, DayOfWeek.Sunday }.Contains(DateTime.Now.DayOfWeek);

        public bool IsServiceAvailable() { return (DateTime.Now.TimeOfDay > new TimeSpan(9, 0, 0)); }

        public List<Quota> GetCurrentQuotes(IList<Etf> etfs)
        {
            var url = "http://213.92.13.40/phprealtime/index.php?modo=t&lang=it&appear=n";

            for (var i = 0; i < etfs.Count(); i++)
            {
                url += $"&id{i + 1}={etfs[i].Isin}";
            }
            url += "&u=43705&p=Z58H50";

            var quote = new List<Quota>();

            var htmlString = GetPageContent(url);
            if (!string.IsNullOrEmpty(htmlString))
            {

                foreach (var etf in etfs)
                {
                    var startIndex = htmlString.IndexOf($"&tlv={etf.Isin}&", StringComparison.Ordinal);
                    if (startIndex == -1)
                        continue;

                    var etfline = htmlString.Substring(startIndex);
                    etfline = etfline.Substring(0, etfline.IndexOf("</tr>", StringComparison.Ordinal));

                    var regex = new Regex($"&tlv={etf.Isin}&.+</a></td><td><b>([0-9,]+)</b></td>.+<font.+>(.+)</font>.+<td.+>[0-9:]{{8}}</td><td>([0-9,.]+)</td>", RegexOptions.IgnoreCase);
                    var match = regex.Match(etfline);

                    if (match.Success)
                    {
                        var value = IsMarketOpen && match.Groups[3].Value != "0.00" ? match.Groups[3].Value : match.Groups[1].Value;
                        var variazione = IsMarketOpen ? "" : match.Groups[2].Value.Trim();

                        decimal chiusura;
                        if (!decimal.TryParse(value, out chiusura))
                            continue;

                        if (chiusura > 0)
                        {
                            quote.Add(new Quota
                            {
                                Ticker = etf.Ticker,
                                Data = DateTime.Now.Date,
                                Chiusura = chiusura,
                                Variazione = variazione,
                                Volumi = -1
                            });
                        }
                    }
                }
            }
            return quote;
        }


        private static string GetPageContent(string url)
        {
            var task = GetPageContentAsync(url);
            task.Wait();

            return task.Result;
        }

        private static async Task<string> GetPageContentAsync(string url)
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
