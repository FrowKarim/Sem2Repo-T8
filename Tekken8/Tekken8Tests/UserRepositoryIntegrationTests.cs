using DAL;
using LogicLayer.Models;
using Tests.Helpers;

namespace Tekken8Tests
{
    public class UserRepositoryIntegrationTests : IntegrationTestBase
    {
        private readonly UserRepository _userRepository;

        public UserRepositoryIntegrationTests()
        {
            var configuration = new TestConfigurationHelper().Configuration;
            _userRepository = new UserRepository(configuration);
        }

        #region AddUser / GetUserByUsername

        [Fact]
        public void AddUser_WithValidUser_InsertsUserSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser1",
                Email = "testuser1@example.com",
                PasswordHash = "hash123",
                TekkenID = "TEKKEN123",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            _userRepository.AddUser(user);
            var result = _userRepository.GetUserByUsername("testuser1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("testuser1", result.Username);
            Assert.Equal("testuser1@example.com", result.Email);
            Assert.Equal("hash123", result.PasswordHash);
            Assert.Equal("TEKKEN123", result.TekkenID);
        }

        #endregion

        #region GetUserById

        

        [Fact]
        public void GetUserById_WithNonExistingId_ReturnsNull()
        {
            // Act
            var result = _userRepository.GetUserById(999999);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetUserByUsername

        [Fact]
        public void GetUserByUsername_WithExistingUsername_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Username = "usernamecheck",
                Email = "usernamecheck@example.com",
                PasswordHash = "hash123",
                TekkenID = "USER123",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);

            // Act
            var result = _userRepository.GetUserByUsername("usernamecheck");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("usernamecheck", result.Username);
        }

        [Fact]
        public void GetUserByUsername_WithUnknownUsername_ReturnsNull()
        {
            // Act
            var result = _userRepository.GetUserByUsername("doesnotexist");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region GetUserByEmail

        [Fact]
        public void GetUserByEmail_WithExistingEmail_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Username = "emailcheck",
                Email = "emailcheck@example.com",
                PasswordHash = "hash123",
                TekkenID = "EMAIL123",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);

            // Act
            var result = _userRepository.GetUserByEmail("emailcheck@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("emailcheck@example.com", result.Email);
        }

        [Fact]
        public void GetUserByEmail_WithUnknownEmail_ReturnsNull()
        {
            // Act
            var result = _userRepository.GetUserByEmail("unknown@example.com");

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateUser

        [Fact]
        public void UpdateUser_WithValidData_UpdatesUserSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "beforeupdate",
                Email = "beforeupdate@example.com",
                PasswordHash = "oldhash",
                TekkenID = "OLD123",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("beforeupdate");

            insertedUser.Username = "afterupdate";
            insertedUser.Email = "afterupdate@example.com";
            insertedUser.PasswordHash = "newhash";
            insertedUser.TekkenID = "NEW123";

            // Act
            _userRepository.UpdateUser(insertedUser);
            var result = _userRepository.GetUserById(insertedUser.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("afterupdate", result.Username);
            Assert.Equal("afterupdate@example.com", result.Email);
            Assert.Equal("newhash", result.PasswordHash);
            Assert.Equal("NEW123", result.TekkenID);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public void DeleteUser_WithExistingUser_RemovesUserSuccessfully()
        {
            // Arrange
            var user = new User
            {
                Username = "deleteuser",
                Email = "deleteuser@example.com",
                PasswordHash = "hash123",
                TekkenID = "DEL123",
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.AddUser(user);
            var insertedUser = _userRepository.GetUserByUsername("deleteuser");

            // Act
            _userRepository.DeleteUser(insertedUser.Id);
            var result = _userRepository.GetUserById(insertedUser.Id);

            // Assert
            Assert.Null(result);
        }

        #endregion
    }
}