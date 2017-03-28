using System;
using RSI.Models;

namespace RSI.ViewModels
{
    public class BilancioFromJson
    {
        public string Data { get; set; }
        public string Invested { get; set; }
        public string Cash { get; set; }
        public string Minusvalenze { get; set; }

        public Bilancio ToBilancio()
        {
            return new Bilancio
            {
                Data = DateTime.Parse(Data),
                Invested = decimal.Parse(Invested),
                Cash = decimal.Parse(Cash),
                Minusvalenze = decimal.Parse(Minusvalenze)
            };
        }
    }
}
