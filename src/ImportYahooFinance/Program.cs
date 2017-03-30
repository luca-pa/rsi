using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RSI.Models;
using RSI.Repositories;
using System.Text;
using RSI.Services;

namespace ImportYahooFinance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var task = new Task(ImportData);
            //task.Start();

            Console.WriteLine($"Started ...");
            ImportFromBorsaItaliana();

            Console.ReadLine();
        }

        public static void ImportFromBorsaItaliana()
        {
            List<string> tickers = getTickers();
            //tickers = new List<string> { "BRES" };
            //tickers = tickers.Where(t => string.Compare(t, "LUSA") > 0).ToList();

            int index = 0;

            foreach (var ticker in tickers)
            {
                var quote = new BorsaItalianaService().GetDailyQuotes(ticker, "3m");
                quote = GetEndOfMonthQuotes(quote);

                if (quote.Any())
                {
                    using (var context = new TradingContext())
                    {
                        var quoteEsistenti = context.Quote.Where(q => q.Ticker == ticker).ToList();

                        foreach (var quota in quote)
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

        private static List<Quota> GetEndOfMonthQuotes(List<Quota> quote)
        {
            var endOfMonthQuotes = new List<Quota>();

            var quoteMese = quote.Where(q => q.Data.ToString("MMyyyy") == DateTime.Now.ToString("MMyyyy"));
            var meseCorrente = quoteMese.OrderBy(q => q.Data).LastOrDefault();
            if (meseCorrente != null)
            {
                meseCorrente.Volumi = (long)quoteMese.Average(q => q.Volumi);
                endOfMonthQuotes.Add(meseCorrente);
            }

            var quoteMesePrecedente = quote.Where(q => q.Data.ToString("MMyyyy") == DateTime.Now.AddMonths(-1).ToString("MMyyyy"));
            var mesePrecedente = quoteMesePrecedente.OrderBy(q => q.Data).LastOrDefault();
            if (mesePrecedente != null)
            {
                mesePrecedente.Volumi = (long)quoteMesePrecedente.Average(q => q.Volumi);
                endOfMonthQuotes.Add(mesePrecedente);
            }
            return endOfMonthQuotes;
        }

        private static List<string> getTickers()
        {
            using (var context = new TradingContext())
            {
                return context.Etfs.Where(e => e.Leveraged == false && e.Active == true).Select(e => e.Ticker).ToList();
            }
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
