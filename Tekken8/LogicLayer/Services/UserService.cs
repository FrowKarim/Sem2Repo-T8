using LogicLayer.Interfaces;
using LogicLayer.Models;
using System;

namespace LogicLayer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("User id must be greater than 0.", nameof(id));
            }

            return _userRepository.GetUserById(id);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username is required.", nameof(username));
            }

            return _userRepository.GetUserByUsername(username);
        }

        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(email));
            }

            return _userRepository.GetUserByEmail(email);
        }

        public User Login(string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));
            }

            return _userRepository.Login(email, passwordHash);
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username is required.", nameof(user.Username));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email is required.", nameof(user.Email));
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException("PasswordHash is required.", nameof(user.PasswordHash));
            }

            user.CreatedAt = DateTime.Now;

            _userRepository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id <= 0)
            {
                throw new ArgumentException("User id must be greater than 0.", nameof(user.Id));
            }

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username is required.", nameof(user.Username));
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("Email is required.", nameof(user.Email));
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new ArgumentException("PasswordHash is required.", nameof(user.PasswordHash));
            }

            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User id must be greater than 0.", nameof(userId));
            }

            _userRepository.DeleteUser(userId);
        }
    }
}