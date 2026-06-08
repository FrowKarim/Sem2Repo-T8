using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LogicLayer.Models;
using LogicLayer.Interfaces;

namespace DAL
{
    public class CharacterRepo : ICharacterRepo
    {
        string connectionString;

        public CharacterRepo(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            
        }

        public List<Character> GetAllCharacters()
        {
            List<Character> CharacterList = new List <Character>();

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand sqlcommand = new SqlCommand("SELECT * FROM Character", connection))
                {

                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Character character = new Character();
                            character.Id = Convert.ToInt32(reader["Id"]);
                            character.Name = reader["characterName"].ToString();
                            character.EditUrl = reader["editUrl"].ToString();
                            character.Game = reader["game"].ToString();

                            CharacterList.Add(character);
                        }
                    }
                }
            }
            return CharacterList;
        }

        public Character GetCharacterById(int id)
        {
            Character character = new Character();
            character.Moves = new List<Move>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                // First, get the character info
                using (SqlCommand sqlcommand = new SqlCommand("SELECT * FROM Character WHERE Id = @Id", conn))
                {
                    sqlcommand.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            character.Id = Convert.ToInt32(reader["Id"]);
                            character.Name = reader["characterName"].ToString();
                            character.EditUrl = reader["editUrl"].ToString();
                            character.Game = reader["game"].ToString();
                        }
                    }
                }

                // Then, get all moves for this character
                using (SqlCommand sqlcommand = new SqlCommand("SELECT * FROM Move WHERE CharacterId = @Id", conn))
                {
                    sqlcommand.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Move move = new Move();
                            move.MoveNumber = Convert.ToInt32(reader["moveNumber"]);
                            move.Command = reader["command"].ToString();
                            move.Name = reader["name"].ToString();
                            move.HitLevel = reader["hitLevel"].ToString();
                            move.Damage = reader["damage"].ToString();
                            move.Startup = reader["startup"].ToString();
                            move.Block = reader["block"].ToString();
                            move.Hit = reader["hit"].ToString();
                            move.CounterHit = reader["counterHit"].ToString();
                            move.Notes = reader["notes"].ToString();
                            move.WavuId = reader["wavuId"].ToString();
                            move.Recovery = reader["recovery"].ToString();
                            move.Image = reader["image"].ToString();
                            move.Video = reader["video"].ToString();
                            move.Transitions = reader["transitionsJson"].ToString().Split(',').ToList();

                            character.Moves.Add(move);
                        }
                    }
                }
            }
            return character;
        }

        public List<Move> GetCharacterMovesById(int id)
        {
            Character character = GetCharacterById(id);
            return character.Moves;
        }
    }
}

