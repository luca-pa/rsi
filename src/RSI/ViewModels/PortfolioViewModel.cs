using RSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSI.ViewModels
{
    public class PortafoglioViewModel
    {
        public List<PortafoglioItemViewModel> Items { get; set; }
        public string TotaleAcquisto { get; set; }
        public string TotaleCorrente { get; set; }
        public string Guadagno { get; set; }
        public string GuadagnoPercentuale { get; set; }
        public string GuadagnoTotale { get; set; }
        public string GuadagnoTotalePercentuale { get; set; }
        public string TotaleComplessivo { get; set; }
        public string TraderLinkUrl { get; set; }
        public string TotaleInvestito { get; set; }
        public string Cash { get; set; }
        public string Minusvalenze { get; set; }
        public string DataBilancio { get; set; }
        public string Variazione { get; set; }

        public PortafoglioViewModel(Portafoglio portafoglio)
        {
            Items = portafoglio.Items.Select(i => new PortafoglioItemViewModel
            {
                Ticker = i.Ticker,
                Isin = i.Etf.Isin,
                Url = i.Etf.Url,
                Nome = i.Nome,
                Data = i.Data.ToString("d"),
                Quantita = i.Quantita.ToString("N0"),
                PrezzoAcquisto = i.Prezzo.ToString("N4"),
                ImportoAcquisto = i.Importo.ToString("N2"),
                PrezzoCorrente = i.PrezzoCorrente.ToString("N4"),
                ImportoCorrenteNetto = i.ImportoCorrenteNetto.ToString("N2"),
                Guadagno = (i.ImportoCorrenteNetto - i.Importo).ToString("N2"),
                GuadagnoPercentuale = ((i.ImportoCorrenteNetto - i.Importo) / i.Importo).ToString("P2"),
                Variazione = i.Variazione,
                Dividendi = i.Dividendi.HasValue ? i.Dividendi.Value.ToString("N2") : string.Empty,
                DataVendita = DateTime.Now.ToString("dd/MM/yyyy"),
                PrezzoVendita = i.PrezzoCorrente.ToString("N4")
            }).OrderByDescending(i => decimal.Parse(i.Guadagno)).ToList();

            var totaleAcquisto = portafoglio.Items.Sum(i => i.Importo);
            var totaleCorrente = portafoglio.Items.Sum(i => i.ImportoCorrenteNetto);
            TotaleAcquisto = totaleAcquisto.ToString("N2");
            TotaleCorrente = totaleCorrente.ToString("N2");
            Guadagno = (totaleCorrente - totaleAcquisto).ToString("N2");
            GuadagnoPercentuale = ((totaleCorrente - totaleAcquisto) / totaleAcquisto).ToString("P2");
            var guadagnoTotale = totaleCorrente + portafoglio.Bilancio.Cash - portafoglio.Bilancio.Invested;
            GuadagnoTotale = guadagnoTotale.ToString("N2");
            GuadagnoTotalePercentuale = (guadagnoTotale / (portafoglio.Bilancio.Invested)).ToString("P2");
            TotaleComplessivo = (portafoglio.Bilancio.Cash + totaleCorrente).ToString("N2");
            TotaleInvestito = portafoglio.Bilancio.Invested.ToString("N2");
            Cash = portafoglio.Bilancio.Cash.ToString("N2");
            Minusvalenze = portafoglio.Bilancio.Minusvalenze.ToString("N2");
            DataBilancio = DateTime.Now.ToString("dd/MM/yyyy");

            var variazioni = portafoglio.Items.Select(i => decimal.Parse(i.Variazione.Replace("%", "")));
            Variazione = variazioni.Any() ? $"{variazioni.Average().ToString("N2")}%" : "";
        }

    }
}
