using System;
using System.Collections.Generic;
using RSI.Models;

namespace RSI.ViewModels
{
    public class EtfChartData
    {
        public string ticker { get; set; }
        public string name { get; set; }
        public IEnumerable<decimal> values { get; set; }
        public IEnumerable<decimal> smas { get; set; }
        public IEnumerable<string> times { get; set; }

        public EtfChartData(RankedEtf etf, DateTime dataRiferimento)
        {
            ticker = etf.Ticker;
            name = etf.Nome;
            times = Times(dataRiferimento);
            smas = Smas(etf);
            values = Quote(etf);
        }

        private IEnumerable<string> Times(DateTime dataRiferimento)
        {
            var timesList = new List<string>();

            for (var i = 13; i >= 1; i--)
            {
                timesList.Add(dataRiferimento.AddMonths(-i).ToString("yyyy-MM-01"));
            }
            return timesList;
        }

        private IEnumerable<decimal> Smas(RankedEtf etf)
        {
            var valuesList = new List<decimal>();

            for (var i = 13; i >= 1; i--)
            {
                valuesList.Add(Math.Round(etf.Sma10Mesi(i).Value, 3));
            }
            return valuesList;
        }

        private IEnumerable<decimal> Quote(RankedEtf etf)
        {
            var valuesList = new List<decimal>();

            for (var i = 13; i >= 1; i--)
            {
                var quota = etf.QuotaMese(i);
                valuesList.Add(quota.HasValue ? quota.Value : 0);
            }
            return valuesList;
        }
    }
}
