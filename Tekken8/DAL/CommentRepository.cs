using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class CommentRepository : ICommentRepository
    {
        private readonly string _connectionString;

        public CommentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Comment> GetCommentsByMoveId(int moveId)
        {
            List<Comment> comments = new List<Comment>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT c.Id, c.MoveId, c.UserId, c.CommentText, c.CreatedAt, c.UpdatedAt,
                       u.Username, u.Email, u.TekkenID
                FROM Comment c
                INNER JOIN [User] u ON c.UserId = u.Id
                WHERE c.MoveId = @MoveId
                ORDER BY c.CreatedAt DESC";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MoveId", moveId);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comments.Add(new Comment
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MoveId = Convert.ToInt32(reader["MoveId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    CommentText = reader["CommentText"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["UpdatedAt"]),
                    User = new User
                    {
                        Id = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        TekkenID = reader["TekkenID"] == DBNull.Value ? null : reader["TekkenID"].ToString()
                    }
                });
            }

            return comments;
        }

        public Comment GetCommentById(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT c.Id, c.MoveId, c.UserId, c.CommentText, c.CreatedAt, c.UpdatedAt,
                       u.Username, u.Email, u.TekkenID
                FROM Comment c
                INNER JOIN [User] u ON c.UserId = u.Id
                WHERE c.Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Comment
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    MoveId = Convert.ToInt32(reader["MoveId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    CommentText = reader["CommentText"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["UpdatedAt"]),
                    User = new User
                    {
                        Id = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        TekkenID = reader["TekkenID"] == DBNull.Value ? null : reader["TekkenID"].ToString()
                    }
                };
            }

            return null;
        }

        public void AddComment(Comment comment)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                INSERT INTO Comment (MoveId, UserId, CommentText, CreatedAt, UpdatedAt)
                VALUES (@MoveId, @UserId, @CommentText, @CreatedAt, @UpdatedAt)";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MoveId", comment.MoveId);
            cmd.Parameters.AddWithValue("@UserId", comment.UserId);
            cmd.Parameters.AddWithValue("@CommentText", comment.CommentText);
            cmd.Parameters.AddWithValue("@CreatedAt", comment.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)comment.UpdatedAt ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateComment(Comment comment)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = @"
                UPDATE Comment
                SET CommentText = @CommentText,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", comment.Id);
            cmd.Parameters.AddWithValue("@CommentText", comment.CommentText);
            cmd.Parameters.AddWithValue("@UpdatedAt", (object?)comment.UpdatedAt ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteComment(int commentId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            string query = "DELETE FROM Comment WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", commentId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}