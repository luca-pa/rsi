using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.ViewModels;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class BilancioController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        public BilancioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpPost]
        public void Post([FromBody] BilancioFromJson bilancioJson)
        {
            _portfolioService.AggiornaBilancio(bilancioJson.ToBilancio());
        }
    }
}
