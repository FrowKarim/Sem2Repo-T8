using DAL;
using Tests.Helpers;

namespace Tekken8Tests
{
    public class CharacterRepositoryIntegrationTests : IntegrationTestBase
    {
        private readonly CharacterRepository _characterRepository;

        public CharacterRepositoryIntegrationTests()
        {
            var configuration = new TestConfigurationHelper().Configuration;
            _characterRepository = new CharacterRepository(configuration);
        }

        #region GetAllCharacters

        [Fact]
        public void GetAllCharacters_WhenDatabaseHasCharacters_ReturnsCharacters()
        {
            // Act
            var result = _characterRepository.GetAllCharacters();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        #endregion

        #region GetCharacterById

        [Fact]
        public void GetCharacterById_WithExistingId_ReturnsCharacter()
        {
            // Arrange
            var allCharacters = _characterRepository.GetAllCharacters();
            var existingCharacterId = allCharacters.First().Id;

            // Act
            var result = _characterRepository.GetCharacterById(existingCharacterId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCharacterId, result.Id);
            Assert.False(string.IsNullOrWhiteSpace(result.Name));
            Assert.NotNull(result.Moves);
        }

        [Fact]
        public void GetCharacterById_WithNonExistingId_ReturnsEmptyCharacter()
        {
            // Act
            var result = _characterRepository.GetCharacterById(999999);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            Assert.Null(result.Name);
            Assert.NotNull(result.Moves);
            Assert.Empty(result.Moves);
        }

        #endregion

        #region GetCharacterMovesById

        [Fact]
        public void GetCharacterMovesById_WithExistingCharacterId_ReturnsMoves()
        {
            // Arrange
            var allCharacters = _characterRepository.GetAllCharacters();
            var existingCharacterId = allCharacters.First().Id;

            // Act
            var result = _characterRepository.GetCharacterMovesById(existingCharacterId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetCharacterMovesById_WithNonExistingCharacterId_ReturnsEmptyList()
        {
            // Act
            var result = _characterRepository.GetCharacterMovesById(999999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetMoveById

        [Fact]
        public void GetMoveById_WithExistingMoveId_ReturnsMove()
        {
            // Arrange
            var allCharacters = _characterRepository.GetAllCharacters();
            var existingCharacter = allCharacters.First();
            var characterWithMoves = _characterRepository.GetCharacterById(existingCharacter.Id);
            var existingMoveId = characterWithMoves.Moves.First().Id;

            // Act
            var result = _characterRepository.GetMoveById(existingMoveId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingMoveId, result.Id);
            Assert.NotNull(result.Transitions);
        }

        [Fact]
        public void GetMoveById_WithNonExistingMoveId_ReturnsNull()
        {
            // Act
            var result = _characterRepository.GetMoveById(999999);

            // Assert
            Assert.Null(result);
        }

        #endregion
    }
}