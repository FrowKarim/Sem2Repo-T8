using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LogicLayer
{
    public class BattleService
    {
        private readonly IEWGFApi _ewgfApi;
        private readonly IMemoryCache _cache;

        public BattleService(IEWGFApi ewgfApi, IMemoryCache cache)
        {
            _ewgfApi = ewgfApi;
            _cache = cache;
        }

        public async Task<List<Battle>> GetBattleDataAsync(string battleId)
        {
            if (string.IsNullOrWhiteSpace(battleId))
            {
                throw new ArgumentException("Battle ID is required.", nameof(battleId));
            }

            var cacheKey = $"battle-data-{battleId}";

            // Return cached battles if available
            if (_cache.TryGetValue(cacheKey, out List<Battle>? cachedBattles) && cachedBattles != null)
            {
                return cachedBattles;
            }

            try
            {
                var battles = await _ewgfApi.GetBattleDataAsync(battleId);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, battles, cacheOptions);

                return battles;
            }
            catch
            {
                // If API fails and cache exists, return cached data
                if (_cache.TryGetValue(cacheKey, out List<Battle>? fallbackBattles) && fallbackBattles != null)
                {
                    return fallbackBattles;
                }

                throw;
            }
        }

        public void CompareBattleData(BattleList Player1, BattleList Player2)
        {
        }

        public double CalculateWinRate(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
            {
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));
            }

            if (battles == null)
            {
                throw new ArgumentNullException(nameof(battles));
            }

            if (battles.Count < 10)
            {
                throw new InvalidOperationException("Not enough battles (minimum 10 required).");
            }

            var wins = battles.Count(b =>
                (string.Equals(b.P1TekkenId, tekkenId, StringComparison.OrdinalIgnoreCase) && b.Winner == 1) ||
                (string.Equals(b.P2TekkenId, tekkenId, StringComparison.OrdinalIgnoreCase) && b.Winner == 2));

            return (double)wins / battles.Count * 100;
        }

        public List<SingleCharacterWinRateStats> GetOwnCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
            {
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));
            }

            if (battles == null)
            {
                throw new ArgumentNullException(nameof(battles));
            }

            return battles
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
                        HasSufficientData = totalGames >= 10
                    };
                })
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }

        public List<SingleCharacterWinRateStats> GetOpponentCharacterWinRates(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
            {
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));
            }

            if (battles == null)
            {
                throw new ArgumentNullException(nameof(battles));
            }

            return battles
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)
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
                        HasSufficientData = totalGames >= 10
                    };
                })
                .OrderByDescending(x => x.TotalGames)
                .ToList();
        }
  

public PlayerBattleSummary GetPlayerBattleSummary(List<Battle> battles, string tekkenId)
        {
            if (string.IsNullOrWhiteSpace(tekkenId))
                throw new ArgumentException("Tekken ID is required.", nameof(tekkenId));

            if (battles == null)
                throw new ArgumentNullException(nameof(battles));

            var playerBattles = battles
                .Where(b => b.P1TekkenId == tekkenId || b.P2TekkenId == tekkenId)
                .ToList();

            if (!playerBattles.Any())
                throw new InvalidOperationException("No battles found for this Tekken ID.");

            var playerName = playerBattles
                .Select(b => b.P1TekkenId == tekkenId ? b.P1Name : b.P2Name)
                .FirstOrDefault();

            var region = playerBattles
                .Select(b => b.P1TekkenId == tekkenId ? b.P1Region : b.P2Region)
                .GroupBy(r => r)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var averageTekkenPower = playerBattles
                .Average(b => b.P1TekkenId == tekkenId ? b.P1TekkenPower : b.P2TekkenPower);

            var averageRoundsWon = playerBattles
                .Average(b => b.P1TekkenId == tekkenId ? b.P1RoundsWon : b.P2RoundsWon);

            var ownCharacterStats = GetOwnCharacterWinRates(playerBattles, tekkenId);
            var opponentCharacterStats = GetOpponentCharacterWinRates(playerBattles, tekkenId);

            var mostPlayedCharacter = ownCharacterStats
                .OrderByDescending(c => c.TotalGames)
                .FirstOrDefault();

            var bestCharacter = ownCharacterStats
                .Where(c => c.TotalGames > 0)
                .OrderByDescending(c => c.WinRate)
                .FirstOrDefault();

            var worstMatchup = opponentCharacterStats
                .Where(c => c.TotalGames > 0)
                .OrderBy(c => c.WinRate)
                .FirstOrDefault();

            var favoriteBattleType = playerBattles
                .GroupBy(b => b.BattleType)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var mostCommonRank = playerBattles
                .Select(b => b.P1TekkenId == tekkenId ? b.P1DanRank : b.P2DanRank)
                .GroupBy(r => r)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return new PlayerBattleSummary
            {
                TekkenId = tekkenId,
                PlayerName = playerName,
                Region = region,
                TotalBattles = playerBattles.Count,
                OverallWinRate = playerBattles.Count >= 10 ? CalculateWinRate(playerBattles, tekkenId) : 0,
                AverageTekkenPower = averageTekkenPower,
                AverageRoundsWon = averageRoundsWon,
                MostPlayedCharacter = mostPlayedCharacter?.CharacterName,
                MostPlayedCharacterGames = mostPlayedCharacter?.TotalGames ?? 0,
                BestCharacter = bestCharacter?.CharacterName,
                BestCharacterWinRate = bestCharacter?.WinRate ?? 0,
                WorstMatchupCharacter = worstMatchup?.CharacterName,
                WorstMatchupWinRate = worstMatchup?.WinRate ?? 0,
                FavoriteBattleType = favoriteBattleType,
                MostCommonRank = mostCommonRank
            };
        }

        public BattleComparisonResult CompareBattleData(List<Battle> player1Battles, string player1TekkenId, List<Battle> player2Battles, string player2TekkenId)
        {
            if (player1Battles == null)
                throw new ArgumentNullException(nameof(player1Battles));

            if (player2Battles == null)
                throw new ArgumentNullException(nameof(player2Battles));
            // maak summary aan voor beide spelers
            var player1Summary = GetPlayerBattleSummary(player1Battles, player1TekkenId);
            var player2Summary = GetPlayerBattleSummary(player2Battles, player2TekkenId);

            return new BattleComparisonResult
            {
                // Vul de vergelijkingresultaten in
                Player1 = player1Summary,
                Player2 = player2Summary,

                // Vergelijk de statistieken en bepaal welke speler beter is op elk gebied
                BetterWinRatePlayer = player1Summary.OverallWinRate > player2Summary.OverallWinRate
                    ? player1Summary.PlayerName
                    : player2Summary.PlayerName,

                BetterTekkenPowerPlayer = player1Summary.AverageTekkenPower > player2Summary.AverageTekkenPower
                    ? player1Summary.PlayerName
                    : player2Summary.PlayerName,

                BetterRoundsWonPlayer = player1Summary.AverageRoundsWon > player2Summary.AverageRoundsWon
                    ? player1Summary.PlayerName
                    : player2Summary.PlayerName,

                MoreConsistentCharacterPlayer = player1Summary.BestCharacterWinRate > player2Summary.BestCharacterWinRate
                    ? player1Summary.PlayerName
                    : player2Summary.PlayerName
            };
        }


    }
}