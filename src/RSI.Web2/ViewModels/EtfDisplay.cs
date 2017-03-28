using RSI.Models;

namespace RSI.ViewModels
{

    public class EtfDisplay
    {
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public int Posizione { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
        public string LineStyle { get; set; }
        public string Media { get; set; }
        public string Volume { get; set; }
        public string TotRet1 { get; set; }
        public string TotRet3 { get; set; }
        public string TotRet6 { get; set; }
        public string TotRet12 { get; set; }
        public string Prezzo { get; set; }
        public string Distribuzione { get; set; }

        public EtfDisplay(int posizione, RankedEtf etf)
        {
            Posizione = posizione;
            Ticker = etf.Ticker;
            Isin = etf.Isin;
            Nome = etf.Nome;
            Url = etf.Url;
            LineStyle = string.Format("{0} {1}", etf.IsTradable ? "" : "nontradable", etf.IsOwned ? "owned" : "");
            Media = etf.MediaTotRet?.ToString("P2");
            Volume = etf.MediaVolumi10.ToString("N0");
            TotRet1 = etf.TotRet1?.ToString("P2");
            TotRet3 = etf.TotRet3?.ToString("P2");
            TotRet6 = etf.TotRet6?.ToString("P2");
            TotRet12 = etf.TotRet12?.ToString("P2");
            Prezzo = etf.PrezzoCorrente?.ToString("N4");
            Distribuzione = etf.Distribuzione ? "D" : "A";
        }
    }
}
