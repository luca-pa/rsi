using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.ViewModels;

namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class RankingController : Controller
    {
        private readonly IRankingService _rankingService;
        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet]
        public IEnumerable<EtfDisplay> Get()
        {
            return Get(DateTime.Now);
        }

        [HttpGet("{data}")]
        public IEnumerable<EtfDisplay> Get(DateTime data)
        {
            int index = 0;
            var ranking = _rankingService.GetSelezione(data);
            return ranking.Etfs
                .OrderByDescending(e => e.MediaTotRet)
                .Select(e => new EtfDisplay(++index, e));
        }

        [HttpPut("{ticker}")]
        public void Put(string ticker)
        {
            _rankingService.AddToSelection(ticker);
        }

        [HttpDelete("{ticker}")]
        public void Delete(string ticker)
        {
            _rankingService.RemoveFromSelection(ticker);
        }
    }
}
