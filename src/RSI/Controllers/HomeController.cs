using System;
using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.ViewModels;

namespace RSI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRankingService _rankingService;
        private readonly IPortfolioService _portfolioService;
        private readonly IEtfService _etfService;

        public HomeController(IRankingService rankingService, IPortfolioService portfolioService, IEtfService etfService)
        {
            _rankingService = rankingService;
            _portfolioService = portfolioService;
            _etfService = etfService;
        }

        public IActionResult Index()
        {
            return Redirect("index.html");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Ranking(DateTime? dataRiferimento = null)
        {
            var ranking = _rankingService.GetSelezione(dataRiferimento);
            var model = RankingViewModel.FromEtfs(ranking);
            model.Title = "Ranking";
            model.Action = nameof(this.Ranking);

            return View("Ranking", model);
        }

        public IActionResult AllRanking(DateTime? dataRiferimento = null)
        {
            var ranking = _rankingService.GetAll(dataRiferimento);
            var model = RankingViewModel.FromEtfs(ranking);
            model.Title = "Ranking (All ETFs)";
            model.Action = nameof(AllRanking);

            return View("Ranking", model);
        }

        public IActionResult Portfolio()
        {
            var items = _portfolioService.GetPortafoglio();
            var portafoglio = new PortafoglioViewModel(items);
            return View(portafoglio);
        }

        public IActionResult AggiornaQuoteSelezione(DateTime? dataRiferimento = null)
        {
            _etfService.AggiornaQuoteSelezione();
            return Ranking(dataRiferimento);
        }

        public IActionResult Charts()
        {
            var datariferimento = DateTime.Now.AddMonths(1);
            var ranking = _rankingService.GetSelezione(datariferimento);
            return View(ranking);
        }
    }
}
