using System.Collections.Generic;

namespace RSI.Models
{
    public class Etf
    {
        public string Ticker { get; set; }
        public string Nome { get; set; }
        public string Indice { get; set; }
        public string Emittente { get; set; }
        public bool Leveraged { get; set; }
        public bool Short { get; set; }
        public bool Distribuzione { get; set; }
        public bool Etn { get; set; }
        public string Isin { get; set; }


        public ICollection<Quota> Quote { get; set; }
        public Selezione Selezione { get; set; }
        public bool IsOwned { get; set; }

        public string Url => string.Format("http://www.borsaitaliana.it/borsa/{0}/scheda/{1}.html", Etn ? "etc-etn" : "etf", Isin);
    }

}
