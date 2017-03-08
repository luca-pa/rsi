using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RSI.Services;
using RSI.ViewModels;


namespace RSI.Controllers
{
    [Route("api/[controller]")]
    public class AllRankingController : Controller
    {
        private readonly IRankingService _rankingService;
        public AllRankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet]
        public IEnumerable<EtfDisplay> Get()
        {
            return Get(DateTime.Now, false);
        }

        [HttpGet("{data}/{shorts}")]
        public IEnumerable<EtfDisplay> Get(DateTime data, bool shorts)
        {
            int index = 0;
            var ranking = _rankingService.GetAll(data, shorts);
            return ranking.Etfs
                .OrderByDescending(e => e.MediaTotRet)
                .Select(e => new EtfDisplay(++index, e));
        }
    }
}
