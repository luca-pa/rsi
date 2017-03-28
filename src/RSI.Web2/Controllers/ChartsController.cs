using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.ViewModels;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class ChartsController : Controller
    {
        private readonly IRankingService _rankingService;
        public ChartsController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet]
        public IEnumerable<EtfChartData> Get()
        {
            var datariferimento = DateTime.Now.AddMonths(1);

            var etfs = _rankingService
                .GetSelezione(datariferimento)
                .Etfs
                .OrderByDescending(e => e.MediaTotRet)
                .Select(e => new EtfChartData(e, datariferimento))
                .ToList();

            return etfs;
        }

    }
    
}
