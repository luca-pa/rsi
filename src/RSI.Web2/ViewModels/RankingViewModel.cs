using RSI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSI.ViewModels
{
    public class RankingViewModel
    {
        public string Title { get; set; }
        public string MeseAnnoCorrente => $"{DataRiferimento.ToString("MMMM")} / {DataRiferimento.Year}";
        public string MeseAnnoPrecedente => $"{DataRiferimento.AddMonths(-1).ToString("MMMM")} / {DataRiferimento.AddMonths(-1).Year}";
        public string MeseAnnoSuccessivo => $"{DataRiferimento.AddMonths(1).ToString("MMMM")} / {DataRiferimento.AddMonths(1).Year}";
        public DateTime DataRiferimento { get; set; }
        public String Action { get; set; }

        public List<EtfDisplay> Etfs { get; set; } = new List<EtfDisplay>();

        public static RankingViewModel FromEtfs(Ranking ranking)
        {
            var model = new RankingViewModel();
            model.DataRiferimento = ranking.DataRiferimento;

            int index = 0;
            ranking.Etfs.OrderByDescending(e => e.MediaTotRet).ToList()
                .ForEach(e => model.Etfs.Add(new EtfDisplay(++index, e)));
            return model;
        }
    }
}

