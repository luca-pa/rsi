using System;
using System.Linq;

namespace RSI.Models
{
    public class RankedEtf : Etf
    {
        public DateTime DataRiferimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public decimal? MediaTotRet => new decimal?[] { TotRet1, TotRet3, TotRet6, TotRet12 }.Average();
        public bool IsTradable => QuotaMese(1) > Sma10Mesi();
        public double MediaVolumi10 { get { var vols = Quote10Mesi().Select(q => q.Volumi); return vols.Any() ? vols.Average() : 0; } }

        public decimal? PrezzoCorrente => QuotaUltimoMese;

        public decimal? TotRet1 => (QuotaUltimoMese - QuotaMesePrecedente) / QuotaMesePrecedente;
        public decimal? TotRet3 => (QuotaUltimoMese - Quota3Mesi) / Quota3Mesi;
        public decimal? TotRet6 => (QuotaUltimoMese - Quota6Mesi) / Quota6Mesi;
        public decimal? TotRet12 => (QuotaUltimoMese - Quota12Mesi) / Quota12Mesi;

        decimal? QuotaUltimoMese => QuotaMese(1);
        decimal? QuotaMesePrecedente => QuotaMese(2);
        decimal? Quota3Mesi => QuotaMese(4);
        decimal? Quota6Mesi => QuotaMese(7);
        decimal? Quota12Mesi => QuotaMese(13);

        public decimal? Sma10Mesi(int numeroMesiPrecedenti = 0)
        {
            var quote = Quote10Mesi(numeroMesiPrecedenti).Select(q => q.Chiusura);
            return quote.Any() ? quote.Average() : 0;
        }

        public decimal? QuotaMese(int numeroMesiPrecedenti)
        {
            return Quote.SingleOrDefault(q => q.Data.ToString("MMyyyy") == DataRiferimento.AddMonths(-numeroMesiPrecedenti).ToString("MMyyyy"))?.Chiusura;
        }

        Quota[] Quote10Mesi(int numeroMesiPrecedenti = 0)
        {
            var data = DataRiferimento.AddMonths(-numeroMesiPrecedenti);
            var endDate = new DateTime(data.Year, data.Month, 1);
            var startDate = endDate.AddMonths(-9);

            return Quote.Where(q => q.Data >= startDate && q.Data <= endDate).ToArray();
        }
    }

}
