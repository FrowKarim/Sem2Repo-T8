using DAL;
using LogicLayer.Models;
using Tests.Helpers;

namespace Tekken8Tests
{
    public class CommentRepositoryIntegrationTests : IntegrationTestBase
    {
        private readonly CommentRepository _commentRepository;
        private readonly UserRepository _userRepository;
        private readonly CharacterRepository _characterRepository;

        public CommentRepositoryIntegrationTests()
        {
            var configuration = new TestConfigurationHelper().Configuration;
            _commentRepository = new CommentRepository(configuration);
            _userRepository = new UserRepository(configuration);
            _characterRepository = new CharacterRepository(configuration);
        }

        #region AddComment / GetCommentById

        

        #endregion

        #region GetCommentsByMoveId

        
        [Fact]
        public void GetCommentsByMoveId_WithUnknownMoveId_ReturnsEmptyList()
        {
            // Act
            var result = _commentRepository.GetCommentsByMoveId(999999);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region UpdateComment

        [Fact]
        public void UpdateComment_WithValidData_UpdatesCommentSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "commentuser3",
                Email = "commentuser3@example.com",
                PasswordHash = "hash123",
                TekkenID = "COMM789",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("commentuser3");

            var character = _characterRepository.GetAllCharacters().First();
            var moveId = _characterRepository.GetCharacterById(character.Id).Moves.First().Id;

            _commentRepository.AddComment(new Comment
            {
                MoveId = moveId,
                UserId = insertedUser.Id,
                CommentText = "Old comment text",
                CreatedAt = DateTime.UtcNow
            });

            var insertedComment = _commentRepository
                .GetCommentsByMoveId(moveId)
                .First(c => c.CommentText == "Old comment text");

            insertedComment.CommentText = "Updated comment text";
            insertedComment.UpdatedAt = DateTime.UtcNow;

            // Act
            _commentRepository.UpdateComment(insertedComment);
            var result = _commentRepository.GetCommentById(insertedComment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated comment text", result.CommentText);
            Assert.NotNull(result.UpdatedAt);
        }

        #endregion

        #region DeleteComment

        

        #endregion

        #region GetCommentById

        [Fact]
        public void GetCommentById_WithUnknownId_ReturnsNull()
        {
            // Act
            var result = _commentRepository.GetCommentById(999999);

            // Assert
            Assert.Null(result);
        }

        #endregion
    }
}