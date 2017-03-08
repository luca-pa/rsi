using Microsoft.AspNetCore.Mvc;
using RSI.Services;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class QuotesController : Controller
    {
        private readonly IEtfService _etfService;
        public QuotesController(IEtfService etfService)
        {
            _etfService = etfService;
        }

        [HttpGet("{all}")]
        public int Get(bool all = false)
        {
            if (all)
            {
                return _etfService.AggiornaQuoteMeseSuccessivo();
            }

            return _etfService.AggiornaQuoteSelezione();

        }
    }
}
