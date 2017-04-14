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
            return Get(DateTime.Now, false, false);
        }

        [HttpGet("etcs/{data}/{shorts}/{distributions}")]
        public IEnumerable<EtfDisplay> GetEtcs(DateTime data, bool shorts, bool distributions)
        {
            return GetAll(data, shorts, distributions, onlyEtcs: true);
        }

        [HttpGet("{data}/{shorts}/{distributions}")]
        public IEnumerable<EtfDisplay> Get(DateTime data, bool shorts, bool distributions)
        {
            return GetAll(data, shorts, distributions);
        }

        private IEnumerable<EtfDisplay> GetAll(DateTime data, bool shorts, bool distributions, bool onlyEtcs = false)
        {
            int index = 0;
            var ranking = onlyEtcs ? _rankingService.GetAllEtcs(data, shorts, distributions) :
                                     _rankingService.GetAll(data, shorts, distributions);
            return ranking.Etfs
                .OrderByDescending(e => e.MediaTotRet)
                .Select(e => new EtfDisplay(++index, e));
        }
    }
}
