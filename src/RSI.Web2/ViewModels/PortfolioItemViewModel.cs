namespace RSI.ViewModels
{
    public class PortafoglioItemViewModel
    {
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string Url { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Quantita { get; set; }
        public string PrezzoAcquisto { get; set; }
        public string ImportoAcquisto { get; set; }
        public string PrezzoCorrente { get; set; }
        public string ImportoCorrenteNetto { get; set; }
        public string Guadagno { get; set; }
        public string GuadagnoPercentuale { get; set; }
        public string Variazione { get; set; }
        public string DataVendita { get; set; }
        public string PrezzoVendita { get; set; }
        public string Dividendi { get; set; }
    }
}
