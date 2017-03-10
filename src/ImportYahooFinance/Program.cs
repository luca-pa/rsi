using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RSI.Models;
using RSI.Repositories;

namespace ImportYahooFinance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var task = new Task(ImportData);
            task.Start();
            Console.WriteLine($"Started ...");

            Console.ReadLine();
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
