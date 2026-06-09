using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User GetUserById(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT Id, Username, Admin, PasswordHash, TekkenID, CreatedAt
                FROM [User]
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        public User GetUserByUsername(string username)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"SELECT Id, Username, Email, PasswordHash, IsAdmin, TekkenID, CreatedAt
            FROM [User]
            WHERE Username = @Username";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        public User GetUserByEmail(string email)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT Id, Username, , PasswordHash, TekkenID, CreatedAt
                FROM [User]
                WHERE Email = @Email";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        public User Login(string email, string passwordHash)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT Id, Username, Email, PasswordHash, TekkenID, CreatedAt
                FROM [User]
                WHERE Email = @Email AND PasswordHash = @PasswordHash";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapUser(reader);
            }

            return null;
        }

        public void AddUser(User user)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                INSERT INTO [User] (Username, Email, PasswordHash, TekkenID, CreatedAt)
                VALUES (@Username, @Email, @PasswordHash, @TekkenID, @CreatedAt)";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@TekkenID", (object?)user.TekkenID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateUser(User user)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                UPDATE [User]
                SET Username = @Username,
                    Email = @Email,
                    PasswordHash = @PasswordHash,
                    TekkenID = @TekkenID
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@TekkenID", (object?)user.TekkenID ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteUser(int userId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "DELETE FROM [User] WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", userId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                Id = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                IsAdmin = Convert.ToBoolean(reader["IsAdmin"]),
                TekkenID = reader["TekkenID"] == DBNull.Value ? null : reader["TekkenID"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}