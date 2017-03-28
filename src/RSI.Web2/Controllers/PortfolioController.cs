using System;
using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.Models;
using RSI.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public PortafoglioViewModel Get()
        {
            var portafoglio = _portfolioService.GetPortafoglio();
            return new PortafoglioViewModel(portafoglio);
        }

        [HttpGet("{storico}")]
        public IEnumerable<PortfolioChartData> Get(bool storico = true)
        {
            return _portfolioService.GetStoricoPerformance().Select(item => new PortfolioChartData(item));
        }

        [HttpPost]
        public void Post([FromBody] PortafoglioItemViewModel item)
        {
            if (string.IsNullOrEmpty(item.Nome))
            {
                DateTime data;
                decimal prezzo;
                int quantita;

                if (DateTime.TryParse(item.Data, out data) && decimal.TryParse(item.PrezzoAcquisto, out prezzo) && int.TryParse(item.Quantita, out quantita))
                {
                    _portfolioService.AddPortafoglioItem(new PortafoglioItem
                    {
                        Ticker = item.Ticker.ToUpper(),
                        Data = data,
                        Prezzo = prezzo,
                        Quantita = quantita,
                        Commissione = 5
                    });
                }
            }
            else
            {
                _portfolioService.UpdatePortafoglioItem(new PortafoglioItem
                {
                    Ticker = item.Ticker,
                    DataVendita = string.IsNullOrEmpty(item.DataVendita) ? (DateTime?)null : DateTime.Parse(item.DataVendita),
                    PrezzoVendita = string.IsNullOrEmpty(item.PrezzoVendita) ? (decimal?)null : decimal.Parse(item.PrezzoVendita),
                    Dividendi = string.IsNullOrEmpty(item.Dividendi) ? (decimal?)null : decimal.Parse(item.Dividendi)
                });
            }
        }
    }
}
