using System;
using System.Collections.Generic;

namespace RSI.Models
{
    public class Portafoglio
    {
        public List<PortafoglioItem> Items { get; set; }
        public Bilancio Bilancio { get; set; }
    }

    public class PortafoglioItem
    {
        public DateTime Data { get; set; }
        public DateTime? DataVendita { get; set; }
        public string Ticker { get; set; }
        public string Nome => Etf?.Nome;
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
        public decimal? PrezzoVendita { get; set; }
        public decimal Commissione { get; set; }
        public decimal? Dividendi { get; set; }
        public string Variazione { get; set; }

        public decimal PrezzoCorrente { get; set; }
        public decimal Importo => Quantita * Prezzo + Commissione;
        public decimal ImportoCorrente => Quantita * PrezzoCorrente - Commissione;
        public decimal Tax => PrezzoCorrente <= Prezzo ? 0 : Etf.Etn ? 0 : Quantita * (PrezzoCorrente - Prezzo) * 26 / 100;
        public decimal ImportoCorrenteNetto => (Quantita * PrezzoCorrente) - Tax - Commissione;

        public Etf Etf { get; set; }
    }

}
