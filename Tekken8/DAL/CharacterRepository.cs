using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly string _connectionString;

        public CharacterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<Character> GetAllCharacters()
        {
            var characterList = new List<Character>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Character", connection))
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var character = new Character
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["characterName"]?.ToString(),
                            EditUrl = reader["editUrl"]?.ToString(),
                            Game = reader["game"]?.ToString(),
                            Moves = new List<Move>()
                        };

                        characterList.Add(character);
                    }
                }
            }

            return characterList;
        }

        public Character GetCharacterById(int id)
        {
            Character? character = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Character WHERE Id = @Id", conn))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            character = new Character
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["characterName"]?.ToString(),
                                EditUrl = reader["editUrl"]?.ToString(),
                                Game = reader["game"]?.ToString(),
                                Moves = new List<Move>()
                            };
                        }
                    }
                }

                if (character == null)
                {
                    return new Character
                    {
                        Moves = new List<Move>()
                    };
                }

                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Move WHERE CharacterId = @Id ORDER BY moveNumber", conn))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transitionsValue = reader["transitionsJson"]?.ToString();

                            var move = new Move
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                MoveNumber = reader["moveNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["moveNumber"]),
                                Command = reader["command"]?.ToString(),
                                Name = reader["name"]?.ToString(),
                                HitLevel = reader["hitLevel"]?.ToString(),
                                Damage = reader["damage"]?.ToString(),
                                Startup = reader["startup"]?.ToString(),
                                Block = reader["block"]?.ToString(),
                                Hit = reader["hit"]?.ToString(),
                                CounterHit = reader["counterHit"]?.ToString(),
                                Notes = reader["notes"]?.ToString(),
                                WavuId = reader["wavuId"]?.ToString(),
                                Recovery = reader["recovery"]?.ToString(),
                                Image = reader["image"]?.ToString(),
                                Video = reader["video"]?.ToString(),
                                Transitions = string.IsNullOrWhiteSpace(transitionsValue)
                                    ? new List<string>()
                                    : transitionsValue.Split(',').ToList()
                            };

                            character.Moves.Add(move);
                        }
                    }
                }
            }

            return character;
        }

        public List<Move> GetCharacterMovesById(int id)
        {
            var character = GetCharacterById(id);
            return character?.Moves ?? new List<Move>();
        }

        public Move? GetMoveById(int moveId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Move WHERE Id = @MoveId", conn))
                {
                    sqlCommand.Parameters.AddWithValue("@MoveId", moveId);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var transitionsValue = reader["transitionsJson"]?.ToString();

                            return new Move
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                MoveNumber = reader["moveNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["moveNumber"]),
                                Command = reader["command"]?.ToString(),
                                Name = reader["name"]?.ToString(),
                                HitLevel = reader["hitLevel"]?.ToString(),
                                Damage = reader["damage"]?.ToString(),
                                Startup = reader["startup"]?.ToString(),
                                Block = reader["block"]?.ToString(),
                                Hit = reader["hit"]?.ToString(),
                                CounterHit = reader["counterHit"]?.ToString(),
                                Notes = reader["notes"]?.ToString(),
                                WavuId = reader["wavuId"]?.ToString(),
                                Recovery = reader["recovery"]?.ToString(),
                                Image = reader["image"]?.ToString(),
                                Video = reader["video"]?.ToString(),
                                Transitions = string.IsNullOrWhiteSpace(transitionsValue)
                                    ? new List<string>()
                                    : transitionsValue.Split(',').ToList()
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}