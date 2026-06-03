using LogicLayer.Interfaces;
using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class BattleService
    {
        private readonly IEWGFApi _ewgfApi;

        public BattleService(IEWGFApi ewgfApi)
        {
            _ewgfApi = ewgfApi;
        }

        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            return await _ewgfApi.GetBattleDataAsync(battleId);
        }

        public double CalculateWinRate(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            if (battles.Count < 10)
                throw new InvalidOperationException("Not enough battles (minimum 10 required).");

            int wins = 0;

            foreach (var battle in battles)
            {
                bool userIsP1 = battle.P1TekkenId == tekkenId;
                bool userIsP2 = battle.P2TekkenId == tekkenId;

                if ((userIsP1 && battle.Winner == 1) || (userIsP2 && battle.Winner == 2))
                {
                    wins++;
                }
            }

            return (double)wins / battles.Count * 100;
        }

        public List<SingleCharacterWinRateStats> GetOwnCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            var result = battles
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P1Char : b.P2Char)
                .Select(group =>
                {
                    int totalGames = 0;
                    int wins = 0;

                    foreach (var battle in group)
                    {
                        totalGames++;

                        bool userIsP1 = battle.P1TekkenId == tekkenId;
                        bool userIsP2 = battle.P2TekkenId == tekkenId;

                        if ((userIsP1 && battle.Winner == 1) || (userIsP2 && battle.Winner == 2))
                        {
                            wins++;
                        }
                    }

                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 10
                    };
                })
                .OrderByDescending(x => x.TotalGames)
                .ToList();

            return result;
        }

        public List<SingleCharacterWinRateStats> GetOpponentCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            var result = battles
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P2Char : b.P1Char)
                .Select(group =>
                {
                    int totalGames = 0;
                    int wins = 0;

                    foreach (var battle in group)
                    {
                        totalGames++;

                        bool userIsP1 = battle.P1TekkenId == tekkenId;
                        bool userIsP2 = battle.P2TekkenId == tekkenId;

                        if ((userIsP1 && battle.Winner == 1) || (userIsP2 && battle.Winner == 2))
                        {
                            wins++;
                        }
                    }

                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 10
                    };
                })
                .OrderByDescending(x => x.TotalGames)
                .ToList();

            return result;
        }
    }
}