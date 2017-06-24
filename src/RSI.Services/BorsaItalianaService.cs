using RSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RSI.Services
{
    public class BorsaItalianaService : IBorsaItalianaService
    {
        const string url = "http://charts.borsaitaliana.it/charts/services/ChartWService.asmx/GetPricesWithVolume";

        public List<Quota> GetDailyQuotesLastMonth(string ticker)
        {
            return GetDailyQuotes(ticker, "1m");
        }

        public List<Quota> GetDailyQuotesLastThreeMonths(string ticker)
        {
            return GetDailyQuotes(ticker, "3m");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="monthRange">"1m", "3m"</param>
        /// <returns></returns>
        private List<Quota> GetDailyQuotes(string ticker, string monthRange)
        {
            var quote = new List<Quota>();

            string responseData = GetUrlContent(url, getRequestBody(ticker, monthRange));

            if (responseData.Contains("Message"))
            {
                return quote;
            }

            responseData = responseData.Replace("{\"d\":[[", "").Replace("]]}", "");
            var items = responseData.Split(new string[] { "],[" }, StringSplitOptions.None);

            foreach (var item in items)
            {
                if (item != null)
                {
                    var values = item.Split(',');

                    if (values.Count() > 1)
                    {
                        quote.Add(new Quota()
                        {
                            Ticker = ticker,
                            Data = FromTicks(values[0]),
                            Chiusura = decimal.Parse(values[1].Replace(".", ",")),
                            Volumi = int.Parse(values[6] == "null" ? "0" : values[6])
                        });
                    }
                }
            }
            return quote;
        }

        private static string getRequestBody(string ticker, string range)
        {
            return JsonConvert.SerializeObject(new
            {
                request = new
                {
                    SampleTime = "1d",
                    TimeFrame = range,
                    RequestedDataSetType = "ohlc",
                    ChartPriceType = "price",
                    Key = $"{ticker}.ETF",
                    OffSet = 0,
                    FromDate = (DateTime?)null,
                    ToDate = (DateTime?)null,
                    UseDelay = true,
                    KeyType = "Topic",
                    KeyType2 = "Topic",
                    Language = "it - IT"
                }
            });
        }

        private static DateTime FromTicks(string ticks)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(ticks));
        }

        private static string GetUrlContent(string url, string bodyContent)
        {
            var task = GetUrlContentAsync(url, bodyContent);
            task.Wait();

            return task.Result;
        }

        private static async Task<string> GetUrlContentAsync(string url, string bodyContent)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.PostAsync(url, new StringContent(bodyContent, Encoding.UTF8, "application/json")))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }

    }
}
