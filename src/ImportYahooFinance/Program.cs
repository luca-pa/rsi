using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RSI.Models;
using RSI.Repositories;
using System.Text;

namespace ImportYahooFinance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var task = new Task(ImportData);
            var task = new Task(ImportFromBorsaItaliana);
            task.Start();
            Console.WriteLine($"Started ...");

            Console.ReadLine();
        }

        public static async void ImportFromBorsaItaliana()
        {
            var url = "http://charts.borsaitaliana.it/charts/services/ChartWService.asmx/GetPricesWithVolume";

            List<string> tickers = getTickers();
            //tickers = new List<string> { "BRES" };
            tickers = tickers.Where(t => string.Compare(t, "LUSA") > 0).ToList();

            int index = 0;

            foreach (var ticker in tickers)
            {
                string responseData;

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.PostAsync(url, new StringContent(getRequestBody(ticker), Encoding.UTF8, "application/json")))
                using (HttpContent content = response.Content)
                {
                    responseData = await content.ReadAsStringAsync();
                }

                if (responseData.Contains("Message"))
                {
                    Console.WriteLine($"{++index}  {ticker}: {responseData}");
                    continue;
                }

                var quoteDaRete = getQuoteFromResponse(responseData, ticker);
                if (quoteDaRete.Any())
                {
                    using (var context = new TradingContext())
                    {
                        var quoteEsistenti = context.Quote.Where(q => q.Ticker == ticker).ToList();

                        foreach (var quota in quoteDaRete)
                        {
                            var quotaEsistente = quoteEsistenti.SingleOrDefault(q => q.Data.ToString("MMyyyy") == quota.Data.ToString("MMyyyy"));

                            if (quotaEsistente == null)
                            {
                                context.Quote.Add(quota);
                            }
                            else if (quotaEsistente.Chiusura != quota.Chiusura || quotaEsistente.Volumi != quota.Volumi)
                            {
                                context.Update(quotaEsistente);
                                quotaEsistente.Chiusura = quota.Chiusura;
                                quotaEsistente.Volumi = quota.Volumi;
                            }
                        }

                        var records = context.SaveChanges();
                        Console.WriteLine($"{++index}  {ticker}: {records} record(s) affected");
                    }
                }
                else
                {
                    Console.WriteLine($"{++index}  {ticker}: ERROR no data returned from web service");
                }
            }

            Console.WriteLine("Done!");
        }

        private static List<Quota> getQuoteFromResponse(string responseData, string ticker)
        {
            if (responseData == "{\"d\":[]}")
            {
                return new List<Quota>();
            }

            responseData = responseData.Replace("{\"d\":[[", "").Replace("]]}", "");
            var items = responseData.Split(new string[] { "],[" }, StringSplitOptions.None);

            var quoteEtf = new List<Quota>();

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var values = item.Split(',');
                    quoteEtf.Add(new Quota()
                    {
                        Ticker = ticker,
                        Data = FromTicks(values[0]),
                        Chiusura = decimal.Parse(values[1].Replace(".", ",")),
                        Volumi = int.Parse(values[6] == "null" ? "0" : values[6])
                    });
                }
            }

            var quote = new List<Quota>();

            var quoteMese = quoteEtf.Where(q => q.Data.ToString("MMyyyy") == DateTime.Now.ToString("MMyyyy"));
            var meseCorrente = quoteMese.OrderBy(q => q.Data).LastOrDefault();
            if (meseCorrente != null)
            {
                meseCorrente.Volumi = (long)quoteMese.Average(q => q.Volumi);
                quote.Add(meseCorrente);
            }

            var quoteMesePrecedente = quoteEtf.Where(q => q.Data.ToString("MMyyyy") == DateTime.Now.AddMonths(-1).ToString("MMyyyy"));
            var mesePrecedente = quoteMesePrecedente.OrderBy(q => q.Data).LastOrDefault();
            if (mesePrecedente != null)
            {
                mesePrecedente.Volumi = (long)quoteMesePrecedente.Average(q => q.Volumi);
                quote.Add(mesePrecedente);
            }

            return quote;
        }

        private static string getRequestBody(string ticker)
        {
            var bodyBefore = "{\"request\":{\"SampleTime\":\"1d\",\"TimeFrame\":\"3m\",\"RequestedDataSetType\":\"ohlc\",\"ChartPriceType\":\"price\",\"Key\":\"";
            var bodyAfter = ".ETF\",\"OffSet\":0,\"FromDate\":null,\"ToDate\":null,\"UseDelay\":true,\"KeyType\":\"Topic\",\"KeyType2\":\"Topic\",\"Language\":\"it - IT\"}}";
            return $"{bodyBefore}{ticker}{bodyAfter}";
        }

        private static List<string> getTickers()
        {
            using (var context = new TradingContext())
            {
                return context.Etfs.Where(e => e.Leveraged == false && e.Active == true).Select(e => e.Ticker).ToList();
            }
        }

        private static DateTime FromTicks(string ticks)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(ticks));
        }

        public static async void ImportData()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-26);

            string url = "http://real-chart.finance.yahoo.com/table.csv?s={0}.MI";
            url += $"&a={startDate.Month - 1}&b={startDate.Day}&c={startDate.Year}&d={endDate.Month - 1}&e={endDate.Day}&f={endDate.Year}&g=m&ignore=.csv";

            List<string> tickers;

            using (var context = new TradingContext())
            {
                tickers = context.Etfs.Where(e => e.Leveraged == false).Select(e => e.Ticker).ToList();
            }

            int index = 0;

            foreach (var ticker in tickers)
            {

                var page = string.Format(url, ticker);

                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    string csvData = await content.ReadAsStringAsync();
                    if (csvData != null)
                    {
                        using (var context = new TradingContext())
                        {

                            var quote = context.Quote.Where(q => q.Ticker == ticker).ToList();

                            var lines = csvData.Split('\n');
                            foreach (var line in lines)
                            {
                                var values = line.Split(',');
                                DateTime data;

                                if (!DateTime.TryParse(values[0], out data))
                                    continue;

                                var chiusura = decimal.Parse(values[6].Replace(".", ","));
                                var volumi = int.Parse(values[5]);

                                var quota = quote.SingleOrDefault(q => q.Data.ToString("MMyyyy") == data.ToString("MMyyyy"));
                                if (quota == null)
                                {
                                    context.Quote.Add(new Quota
                                    {
                                        Ticker = ticker.Trim(),
                                        Data = data,
                                        Chiusura = chiusura,
                                        Volumi = volumi
                                    });
                                }
                                else if (chiusura != quota.Chiusura || volumi != quota.Volumi)
                                {
                                    context.Update(quota);
                                    quota.Chiusura = chiusura;
                                    quota.Volumi = volumi;
                                }

                            }

                            var records = context.SaveChanges();
                            Console.WriteLine($"{++index}  {ticker}: {records} record(s) affected");
                        }
                    }
                }
            }
            Console.WriteLine("Done!");

        }
    }
}
