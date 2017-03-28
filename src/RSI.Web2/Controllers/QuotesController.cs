using Microsoft.AspNetCore.Mvc;
using RSI.Services;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class QuotesController : Controller
    {
        private readonly IEtfService _etfService;
        private readonly IPortfolioService _portfolioService;
        public QuotesController(IEtfService etfService, IPortfolioService portfolioService)
        {
            _etfService = etfService;
            _portfolioService = portfolioService;
        }

        [HttpGet()]
        public int Get()
        {
            return _etfService.AggiornaQuoteSelezione();
        }

        [HttpGet("{all}")]
        public int Get(bool all = true)
        {
            return _etfService.AggiornaQuoteMeseSuccessivo();
        }

        [HttpGet("{all}/{portfolio}")]
        public int Get(bool all = false, bool portfolio = true)
        {
            return _portfolioService.AggiornaQuotePortfolio();
        }
    }
}
