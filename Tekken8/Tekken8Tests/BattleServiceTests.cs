using Moq;
using DAL;
using LogicLayer;
using LogicLayer.Interfaces;
using LogicLayer.Models;
using LogicLayer.Services;
using Microsoft.Extensions.Caching.Memory;


namespace Tekken8Tests
{
    public class BattleServiceTests
    {
        private readonly Mock<IEWGFApi> _mockApi;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly BattleService _battleService;

        public BattleServiceTests()
        {
            _mockApi = new Mock<IEWGFApi>();    
            _mockCache = new Mock<IMemoryCache>();
            _battleService = new BattleService(_mockApi.Object, _mockCache.Object);
        }

        #region CalculateWinRate Tests

        [Fact]
        public void CalculateWinRate_WithValidData_ReturnsCorrectWinRate()
        {
            // Arrange
            var tekkenId = "TestPlayer123";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent3", Winner = 2 },
                new Battle { P1TekkenId = "Opponent4", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent5", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent6", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent8", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent10", Winner = 2 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(60.0, result);
        }

        [Fact]
        public void CalculateWinRate_WithPerfectWinRate_Returns100()
        {
            // Arrange
            var tekkenId = "UndefeatedPlayer";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = "Opponent3", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent4", Winner = 1 },
                new Battle { P1TekkenId = "Opponent5", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent6", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 1 },
                new Battle { P1TekkenId = "Opponent8", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent10", Winner = 1 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(100.0, result);
        }

        [Fact]
        public void CalculateWinRate_WithNoWins_Returns0()
        {
            // Arrange
            var tekkenId = "LoosingPlayer";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 2 },
                new Battle { P1TekkenId = "Opponent3", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent4", Winner = 2 },
                new Battle { P1TekkenId = "Opponent5", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent6", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 2 },
                new Battle { P1TekkenId = "Opponent8", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent10", Winner = 2 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(0.0, result);
        }

        

        [Fact]
        public void CalculateWinRate_WithNullTekkenId_ThrowsArgumentException()
        {
            // Arrange
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = "Player1", P2TekkenId = "Player2", Winner = 1 }
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _battleService.CalculateTotalWinRate(battles, null));
        }

        [Fact]
        public void CalculateWinRate_WithEmptyTekkenId_ThrowsArgumentException()
        {
            // Arrange
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = "Player1", P2TekkenId = "Player2", Winner = 1 }
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _battleService.CalculateTotalWinRate(battles, ""));
        }

        [Fact]
        public void CalculateWinRate_WithWhitespaceTekkenId_ThrowsArgumentException()
        {
            // Arrange
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = "Player1", P2TekkenId = "Player2", Winner = 1 }
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _battleService.CalculateTotalWinRate(battles, "   "));
        }

        [Fact]
        public void CalculateWinRate_WithNullBattlesList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _battleService.CalculateTotalWinRate(null, "TestPlayer"));
        }

        [Fact]
        public void CalculateWinRate_WithFewerThan10Battles_ThrowsInvalidOperationException()
        {
            // Arrange
            var tekkenId = "TestPlayer";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent3", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent4", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent5", Winner = 1 }
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _battleService.CalculateTotalWinRate(battles, tekkenId));
        }

        [Fact]
        public void CalculateWinRate_WithExactly10Battles_CalculatesSuccessfully()
        {
            // Arrange
            var tekkenId = "TestPlayer";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent3", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent4", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent5", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent6", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent8", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent10", Winner = 2 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(50.0, result);
        }

        [Fact]
        public void CalculateWinRate_WhenPlayerIsOnlyP1_CalculatesCorrectly()
        {
            // Arrange
            var tekkenId = "OnlyP1Player";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent3", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent4", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent5", Winner = 1 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent6", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent8", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 2 },
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent10", Winner = 2 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(50.0, result);
        }

        [Fact]
        public void CalculateWinRate_WhenPlayerIsOnlyP2_CalculatesCorrectly()
        {
            // Arrange
            var tekkenId = "OnlyP2Player";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = "Opponent1", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent2", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent3", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent4", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent5", P2TekkenId = tekkenId, Winner = 2 },
                new Battle { P1TekkenId = "Opponent6", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = "Opponent7", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = "Opponent8", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = "Opponent9", P2TekkenId = tekkenId, Winner = 1 },
                new Battle { P1TekkenId = "Opponent10", P2TekkenId = tekkenId, Winner = 1 }
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(50.0, result);
        }

        [Fact]
        public void CalculateWinRate_WithMixedP1AndP2_CalculatesCorrectly()
        {
            // Arrange
            var tekkenId = "MixedPositionPlayer";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent1", Winner = 1 },  // Win as P1
                new Battle { P1TekkenId = "Opponent2", P2TekkenId = tekkenId, Winner = 2 },  // Win as P2
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent3", Winner = 1 },  // Win as P1
                new Battle { P1TekkenId = "Opponent4", P2TekkenId = tekkenId, Winner = 2 },  // Win as P2
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent5", Winner = 1 },  // Win as P1
                new Battle { P1TekkenId = "Opponent6", P2TekkenId = tekkenId, Winner = 1 },  // Loss as P2
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent7", Winner = 2 },  // Loss as P1
                new Battle { P1TekkenId = "Opponent8", P2TekkenId = tekkenId, Winner = 1 },  // Loss as P2
                new Battle { P1TekkenId = tekkenId, P2TekkenId = "Opponent9", Winner = 1 },  // Win as P1
                new Battle { P1TekkenId = "Opponent10", P2TekkenId = tekkenId, Winner = 1 }  // Loss as P2
            };

            // Act
            var result = _battleService.CalculateTotalWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(60.0, result);
        }

        #endregion
        

        #region GetBattleDataAsync

        
        [Fact]
        public async Task GetBattleDataAsync_WithNullBattleId_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _battleService.GetBattleDataAsync(null));
        }

        [Fact]
        public async Task GetBattleDataAsync_WithEmptyBattleId_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _battleService.GetBattleDataAsync(""));
        }

        [Fact]
        public async Task GetBattleDataAsync_WithWhitespaceBattleId_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _battleService.GetBattleDataAsync(" "));
        }

        #endregion

        #region GetOwnCharacterWinRates

        [Fact]
        public void GetOwnCharacterWinRates_ReturnsCorrectCharacterStatistics()
        {
            var battles = new List<Battle>
            {
                new Battle
                {
                    P1TekkenId = "Player1",
                    P1Char = "Jin",
                    Winner = 1
                },
                new Battle
                {
                    P1TekkenId = "Player1",
                    P1Char = "Jin",
                    Winner = 1
                },
                new Battle
                {
                    P1TekkenId = "Player1",
                    P1Char = "Jin",
                    Winner = 2
                },
                new Battle
                {
                    P1TekkenId = "Player1",
                    P1Char = "Kazuya",
                    Winner = 1
                }
            };

            var result = _battleService.GetOwnCharacterWinRates(battles, "Player1");

            Assert.Equal(2, result.Count);

            var jin = result.First(x => x.CharacterName == "Jin");

            Assert.Equal(3, jin.TotalGames);
            Assert.Equal(2, jin.Wins);
            Assert.Equal(66.66666666666666, jin.WinRate);
            Assert.False(jin.HasSufficientData);
        }

        [Fact]
        public void GetOwnCharacterWinRates_WithTenGames_SetsHasSufficientDataTrue()
        {
            var battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = "Player1",
                    P1Char = "Jin",
                    Winner = 1
                });
            }

            var result = _battleService.GetOwnCharacterWinRates(battles, "Player1");

            Assert.Single(result);
            Assert.True(result.First().HasSufficientData);
            Assert.Equal(10, result.First().TotalGames);
        }

        [Fact]
        public void GetOwnCharacterWinRates_WithNullBattles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _battleService.GetOwnCharacterWinRates(null, "Player1"));
        }

        [Fact]
        public void GetOwnCharacterWinRates_WithEmptyTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetOwnCharacterWinRates(new List<Battle>(), ""));
        }

        [Fact]
        public void GetOwnCharacterWinRates_WithWhitespaceTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetOwnCharacterWinRates(new List<Battle>(), " "));
        }

        #endregion

        #region GetOpponentCharacterWinRates

        [Fact]
        public void GetOpponentCharacterWinRates_ReturnsCorrectStatistics()
        {
            var battles = new List<Battle>
            {
                new Battle
                {
                    P1TekkenId = "Player1",
                    P2Char = "King",
                    Winner = 1
                },
                new Battle
                {
                    P1TekkenId = "Player1",
                    P2Char = "King",
                    Winner = 2
                },
                new Battle
                {
                    P1TekkenId = "Player1",
                    P2Char = "Paul",
                    Winner = 1
                }
            };

            var result = _battleService.GetOpponentCharacterWinRates(battles, "Player1");

            Assert.Equal(2, result.Count);

            var king = result.First(x => x.CharacterName == "King");

            Assert.Equal(2, king.TotalGames);
            Assert.Equal(1, king.Wins);
            Assert.Equal(50, king.WinRate);
        }

        [Fact]
        public void GetOpponentCharacterWinRates_WithTenGames_HasSufficientDataTrue()
        {
            var battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = "Player1",
                    P2Char = "King",
                    Winner = 1
                });
            }

            var result = _battleService.GetOpponentCharacterWinRates(battles, "Player1");

            Assert.Single(result);
            Assert.True(result.First().HasSufficientData);
            Assert.Equal(10, result.First().TotalGames);
        }

        [Fact]
        public void GetOpponentCharacterWinRates_WithNullBattles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _battleService.GetOpponentCharacterWinRates(null, "Player1"));
        }

        [Fact]
        public void GetOpponentCharacterWinRates_WithNullTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetOpponentCharacterWinRates(new List<Battle>(), null));
        }

        [Fact]
        public void GetOpponentCharacterWinRates_WithEmptyTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetOpponentCharacterWinRates(new List<Battle>(), ""));
        }

        #endregion

        #region GetPlayerBattleSummary

        [Fact]
        public void GetPlayerBattleSummary_WithValidData_ReturnsCorrectSummary()
        {
            var tekkenId = "Player1";

            var battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = tekkenId,
                    P1Name = "TestPlayer",
                    P1Region = "EU",
                    P1TekkenPower = 150000,
                    P1RoundsWon = 2,
                    P1Char = "Jin",
                    P1DanRank = "Tekken King",
                    BattleType = "Ranked",
                    Winner = 1
                });
            }

            var result = _battleService.GetPlayerBattleSummary(battles, tekkenId);

            Assert.Equal(tekkenId, result.TekkenId);
            Assert.Equal("TestPlayer", result.PlayerName);
            Assert.Equal("EU", result.Region);
            Assert.Equal(10, result.TotalBattles);
            Assert.Equal(100, result.OverallWinRate);
            Assert.Equal(150000, result.AverageTekkenPower);
            Assert.Equal(2, result.AverageRoundsWon);
            Assert.Equal("Jin", result.MostPlayedCharacter);
            Assert.Equal(10, result.MostPlayedCharacterGames);
            Assert.Equal("Jin", result.BestCharacter);
            Assert.Equal(100, result.BestCharacterWinRate);
            Assert.Equal("Ranked", result.FavoriteBattleType);
            Assert.Equal("Tekken King", result.MostCommonRank);
        }

        [Fact]
        public void GetPlayerBattleSummary_WithMixedCharacters_ReturnsMostPlayedCharacter()
        {
            var tekkenId = "Player1";

            var battles = new List<Battle>();

            for (int i = 0; i < 7; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = tekkenId,
                    P1Name = "Player",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Jin",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = 1
                });
            }

            for (int i = 0; i < 3; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = tekkenId,
                    P1Name = "Player",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Kazuya",
                    P1DanRank = "Raijin",
                    BattleType = "Quick Match",
                    Winner = 2
                });
            }

            var result = _battleService.GetPlayerBattleSummary(battles, tekkenId);

            Assert.Equal("Jin", result.MostPlayedCharacter);
            Assert.Equal(7, result.MostPlayedCharacterGames);
            Assert.Equal(70, result.OverallWinRate);
        }

        [Fact]
        public void GetPlayerBattleSummary_WithLessThan10Battles_ReturnsZeroOverallWinRate()
        {
            var tekkenId = "Player1";

            var battles = new List<Battle>();

            for (int i = 0; i < 5; i++)
            {
                battles.Add(new Battle
                {
                    P1TekkenId = tekkenId,
                    P1Name = "Player",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Jin",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = 1
                });
            }

            var result = _battleService.GetPlayerBattleSummary(battles, tekkenId);

            Assert.Equal(5, result.TotalBattles);
            Assert.Equal(0, result.OverallWinRate);
        }

        [Fact]
        public void GetPlayerBattleSummary_WithNullBattles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _battleService.GetPlayerBattleSummary(null, "Player1"));
        }

        [Fact]
        public void GetPlayerBattleSummary_WithNullTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetPlayerBattleSummary(new List<Battle>(), null));
        }

        [Fact]
        public void GetPlayerBattleSummary_WithEmptyTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetPlayerBattleSummary(new List<Battle>(), ""));
        }

        [Fact]
        public void GetPlayerBattleSummary_WithWhitespaceTekkenId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _battleService.GetPlayerBattleSummary(new List<Battle>(), " "));
        }

        [Fact]
        public void GetPlayerBattleSummary_WithNoMatchingBattles_ThrowsInvalidOperationException()
        {
            var battles = new List<Battle>
    {
        new Battle
        {
            P1TekkenId = "SomeoneElse",
            P2TekkenId = "AnotherPlayer"
        }
    };

            Assert.Throws<InvalidOperationException>(() =>
                _battleService.GetPlayerBattleSummary(battles, "Player1"));
        }

        #endregion
        


        #region CompareBattleData

[Fact]
public void CompareBattleData_WithValidData_ReturnsCorrectComparison()
        {
            var player1Battles = new List<Battle>();
            var player2Battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                player1Battles.Add(new Battle
                {
                    P1TekkenId = "Player1",
                    P1Name = "Player One",
                    P1Region = "EU",
                    P1TekkenPower = 200000,
                    P1RoundsWon = 3,
                    P1Char = "Jin",
                    P1DanRank = "Tekken King",
                    BattleType = "Ranked",
                    Winner = i < 8 ? 1 : 2
                });

                player2Battles.Add(new Battle
                {
                    P1TekkenId = "Player2",
                    P1Name = "Player Two",
                    P1Region = "NA",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Paul",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = i < 5 ? 1 : 2
                });
            }

            var result = _battleService.CompareBattleData(
                player1Battles,
                "Player1",
                player2Battles,
                "Player2");

            Assert.NotNull(result);
            Assert.Equal("Player One", result.Player1.PlayerName);
            Assert.Equal("Player Two", result.Player2.PlayerName);

            Assert.Equal("Player One", result.BetterWinRatePlayer);
            Assert.Equal("Player One", result.BetterTekkenPowerPlayer);
            Assert.Equal("Player One", result.BetterRoundsWonPlayer);
            Assert.Equal("Player One", result.MoreConsistentCharacterPlayer);
        }

        [Fact]
        public void CompareBattleData_WithEqualWinRates_ReturnsSecondPlayer()
        {
            var player1Battles = new List<Battle>();
            var player2Battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                player1Battles.Add(new Battle
                {
                    P1TekkenId = "Player1",
                    P1Name = "Player One",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Jin",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = i < 5 ? 1 : 2
                });

                player2Battles.Add(new Battle
                {
                    P1TekkenId = "Player2",
                    P1Name = "Player Two",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Paul",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = i < 5 ? 1 : 2
                });
            }

            var result = _battleService.CompareBattleData(
                player1Battles,
                "Player1",
                player2Battles,
                "Player2");

            Assert.Equal("Player Two", result.BetterWinRatePlayer);
        }

        [Fact]
        public void CompareBattleData_WithNullPlayer1Battles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _battleService.CompareBattleData(
                    null,
                    "Player1",
                    new List<Battle>(),
                    "Player2"));
        }

        [Fact]
        public void CompareBattleData_WithNullPlayer2Battles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _battleService.CompareBattleData(
                    new List<Battle>(),
                    "Player1",
                    null,
                    "Player2"));
        }

        [Fact]
        public void CompareBattleData_WhenPlayerOneHasHigherTekkenPower_ReturnsPlayerOne()
        {
            var player1Battles = new List<Battle>();
            var player2Battles = new List<Battle>();

            for (int i = 0; i < 10; i++)
            {
                player1Battles.Add(new Battle
                {
                    P1TekkenId = "Player1",
                    P1Name = "Player One",
                    P1Region = "EU",
                    P1TekkenPower = 300000,
                    P1RoundsWon = 2,
                    P1Char = "Jin",
                    P1DanRank = "Tekken King",
                    BattleType = "Ranked",
                    Winner = 1
                });

                player2Battles.Add(new Battle
                {
                    P1TekkenId = "Player2",
                    P1Name = "Player Two",
                    P1Region = "EU",
                    P1TekkenPower = 100000,
                    P1RoundsWon = 2,
                    P1Char = "Paul",
                    P1DanRank = "Raijin",
                    BattleType = "Ranked",
                    Winner = 1
                });
            }

            var result = _battleService.CompareBattleData(
                player1Battles,
                "Player1",
                player2Battles,
                "Player2");

            Assert.Equal("Player One", result.BetterTekkenPowerPlayer);
        }

#endregion


    }
}

