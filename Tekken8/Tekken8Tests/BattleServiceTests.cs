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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void CalculateWinRate_WithCaseInsensitiveTekkenId_CalculatesCorrectly()
        {
            // Arrange
            var tekkenId = "TestPlayer123";
            var battles = new List<Battle>
            {
                new Battle { P1TekkenId = "TESTPLAYER123", P2TekkenId = "Opponent1", Winner = 1 },
                new Battle { P1TekkenId = "testplayer123", P2TekkenId = "Opponent2", Winner = 1 },
                new Battle { P1TekkenId = "TestPlayer123", P2TekkenId = "Opponent3", Winner = 2 },
                new Battle { P1TekkenId = "Opponent4", P2TekkenId = "TESTPLAYER123", Winner = 2 },
                new Battle { P1TekkenId = "Opponent5", P2TekkenId = "testplayer123", Winner = 1 },
                new Battle { P1TekkenId = "TestPlayer123", P2TekkenId = "Opponent6", Winner = 1 },
                new Battle { P1TekkenId = "testplayer123", P2TekkenId = "Opponent7", Winner = 2 },
                new Battle { P1TekkenId = "TestPlayer123", P2TekkenId = "Opponent8", Winner = 1 },
                new Battle { P1TekkenId = "TESTPLAYER123", P2TekkenId = "Opponent9", Winner = 1 },
                new Battle { P1TekkenId = "testplayer123", P2TekkenId = "Opponent10", Winner = 2 }
            };

            // Act
            var result = _battleService.CalculateWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(60.0, result);
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
            Assert.Throws<ArgumentException>(() => _battleService.CalculateWinRate(battles, null));
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
            Assert.Throws<ArgumentException>(() => _battleService.CalculateWinRate(battles, ""));
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
            Assert.Throws<ArgumentException>(() => _battleService.CalculateWinRate(battles, "   "));
        }

        [Fact]
        public void CalculateWinRate_WithNullBattlesList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _battleService.CalculateWinRate(null, "TestPlayer"));
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
            Assert.Throws<InvalidOperationException>(() => _battleService.CalculateWinRate(battles, tekkenId));
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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

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
            var result = _battleService.CalculateWinRate(battles, tekkenId);

            // Assert
            Assert.Equal(60.0, result);
        }

        #endregion
    }
}

