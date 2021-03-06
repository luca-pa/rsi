﻿using System;
using RSI.Models;

namespace RSI.Services
{
    public interface IRankingService
    {
        Ranking GetAll(DateTime? dataRiferimento = default(DateTime?), bool shorts = false, bool getDistribution = false, int minVolumes = 5000);
        Ranking GetAllEtcs(DateTime? dataRiferimento = default(DateTime?), bool shorts = false, bool getDistribution = false, int minVolumes = 5000);
        Ranking GetSelezione(DateTime? dataRiferimento = default(DateTime?));
        void RemoveFromSelection(string ticker);
        void AddToSelection(string ticker);
    }
}