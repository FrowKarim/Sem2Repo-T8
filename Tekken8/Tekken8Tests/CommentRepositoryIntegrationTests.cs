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

        [Fact]
        public void AddComment_WithValidData_InsertsCommentSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "commentuser1",
                Email = "commentuser1@example.com",
                PasswordHash = "hash123",
                TekkenID = "COMM123",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("commentuser1");

            var character = _characterRepository.GetAllCharacters().First();
            var moveId = _characterRepository.GetCharacterById(character.Id).Moves.First().Id;

            var comment = new Comment
            {
                MoveId = moveId,
                UserId = insertedUser.Id,
                CommentText = "Integration test comment",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            // Act
            _commentRepository.AddComment(comment);
            var comments = _commentRepository.GetCommentsByMoveId(moveId);
            var result = comments.FirstOrDefault(c => c.CommentText == "Integration test comment");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(moveId, result.MoveId);
            Assert.Equal(insertedUser.Id, result.UserId);
        }

        #endregion

        #region GetCommentsByMoveId

        [Fact]
        public void GetCommentsByMoveId_WithExistingComments_ReturnsComments()
        {
            // Arrange
            var user = new User
            {
                Username = "commentuser2",
                Email = "commentuser2@example.com",
                PasswordHash = "hash123",
                TekkenID = "COMM456",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("commentuser2");

            var character = _characterRepository.GetAllCharacters().First();
            var moveId = _characterRepository.GetCharacterById(character.Id).Moves.First().Id;

            _commentRepository.AddComment(new Comment
            {
                MoveId = moveId,
                UserId = insertedUser.Id,
                CommentText = "First comment",
                CreatedAt = DateTime.UtcNow
            });

            _commentRepository.AddComment(new Comment
            {
                MoveId = moveId,
                UserId = insertedUser.Id,
                CommentText = "Second comment",
                CreatedAt = DateTime.UtcNow.AddMinutes(1)
            });

            // Act
            var result = _commentRepository.GetCommentsByMoveId(moveId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

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

        [Fact]
        public void DeleteComment_WithExistingComment_RemovesCommentSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "commentuser4",
                Email = "commentuser4@example.com",
                PasswordHash = "hash123",
                TekkenID = "COMM999",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("commentuser4");

            var character = _characterRepository.GetAllCharacters().First();
            var moveId = _characterRepository.GetCharacterById(character.Id).Moves.First().Id;

            _commentRepository.AddComment(new Comment
            {
                MoveId = moveId,
                UserId = insertedUser.Id,
                CommentText = "Comment to delete",
                CreatedAt = DateTime.UtcNow
            });

            var insertedComment = _commentRepository
                .GetCommentsByMoveId(moveId)
                .First(c => c.CommentText == "Comment to delete");

            // Act
            _commentRepository.DeleteComment(insertedComment.Id);
            var result = _commentRepository.GetCommentById(insertedComment.Id);

            // Assert
            Assert.Null(result);
        }

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