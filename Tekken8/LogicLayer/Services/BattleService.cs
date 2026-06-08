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

            
            var wins = battles.Count(b =>
                (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                (b.P2TekkenId == tekkenId && b.Winner == 2));

            
            return (double)wins / battles.Count * 100;
        }

        
        public List<SingleCharacterWinRateStats> GetOwnCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            return battles
                // Only include battles where this Tekken ID participated
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)

               
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P1Char : b.P2Char)

                .Select(group =>
                {
                    
                    var totalGames = group.Count();

                    
                    var wins = group.Count(b =>
                        (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                        (b.P2TekkenId == tekkenId && b.Winner == 2));

                    
                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 5
                    };
                })

                
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }

        public List<SingleCharacterWinRateStats> GetOpponentCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            return battles
                
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)

                // Group by the opponent's character
                .GroupBy(b => b.P1TekkenId == tekkenId ? b.P2Char : b.P1Char)

                .Select(group =>
                {
                    var totalGames = group.Count();

                    
                    var wins = group.Count(b =>
                        (b.P1TekkenId == tekkenId && b.Winner == 1) ||
                        (b.P2TekkenId == tekkenId && b.Winner == 2));

                    return new SingleCharacterWinRateStats
                    {
                        CharacterName = group.Key,
                        TotalGames = totalGames,
                        Wins = wins,
                        WinRate = totalGames == 0 ? 0 : (double)wins / totalGames * 100,
                        HasSufficientData = totalGames >= 5
                    };
                })

                
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }


    }
}