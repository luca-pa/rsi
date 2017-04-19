using System;
using System.Collections.Generic;

namespace RSI.Models
{
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
        public decimal? UltimaChiusura { get; set; }

        public decimal Importo => Quantita * Prezzo + Commissione;
        public decimal ImportoCorrente => Quantita * PrezzoCorrente - Commissione;
        public decimal Tax => PrezzoCorrente <= Prezzo ? 0 : Etf.Etn ? 0 : Quantita * (PrezzoCorrente - Prezzo) * 26 / 100;
        public decimal ImportoCorrenteNetto => (Quantita * PrezzoCorrente) - Tax - Commissione;

        public Etf Etf { get; set; }


        public void SetCurrentPrice(Quota quota)
        {
            if (quota == null)
            {
                PrezzoCorrente = UltimaChiusura ?? 0;
                return;
            }

            PrezzoCorrente = quota.Chiusura;
            if (string.IsNullOrEmpty(quota.Variazione) && UltimaChiusura.HasValue)
            {
                Variazione = ((quota.Chiusura - UltimaChiusura) / UltimaChiusura).Value.ToString("P2");
            }
            else
            {
                Variazione = quota.Variazione ?? 0.ToString("P2");
            }
        }

    }

}
