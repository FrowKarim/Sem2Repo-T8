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
        // API dependency used to retrieve battle data
        private readonly IEWGFApi _ewgfApi;

        public BattleService(IEWGFApi ewgfApi)
        {
            _ewgfApi = ewgfApi;
        }

        // Gets battle data from the API for the given battle/player id
        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            return await _ewgfApi.GetBattleDataAsync(battleId);
        }

        // Calculates the overall win rate for the given Tekken ID
        public double CalculateWinRate(List<Battle> battles, string tekkenId)
        {
            // Validate required input
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            // Require at least 10 battles for meaningful statistics
            if (battles.Count < 10)
                throw new InvalidOperationException("Not enough battles (minimum 10 required).");

            // Count battles where the user won as player 1 or player 2
            var wins = battles.Count(b =>
                (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                (b.P2TekkenId == tekkenId && b.Winner == 2));

            // Return win percentage
            return (double)wins / battles.Count * 100;
        }

        // Returns win rate stats grouped by the character the user played
        public List<SingleCharacterWinRateStats> GetOwnCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            // Validate required input
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            return battles
                // Only include battles where this Tekken ID participated
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)

                // Group by the user's own character
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P1Char : b.P2Char)

                .Select(group =>
                {
                    // Total amount of games played with this character
                    var totalGames = group.Count();

                    // Total wins with this character
                    var wins = group.Count(b =>
                        (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                        (b.P2TekkenId == tekkenId && b.Winner == 2));

                    // Build the stats object for this character
                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 5
                    };
                })

                // Show most played characters first
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }

        // Returns win rate stats grouped by the opponent's character
        public List<SingleCharacterWinRateStats> GetOpponentCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            // Validate required input
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            return battles
                // Only include battles where this Tekken ID participated
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)

                // Group by the opponent's character
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P2Char : b.P1Char)

                .Select(group =>
                {
                    // Total amount of games played against this character
                    var totalGames = group.Count();

                    // Total wins against this opponent character
                    var wins = group.Count(b =>
                        (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                        (b.P2TekkenId == tekkenId && b.Winner == 2));

                    // Build the stats object for this opponent character
                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 5
                    };
                })

                // Show most encountered opponent characters first
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }
    }
}