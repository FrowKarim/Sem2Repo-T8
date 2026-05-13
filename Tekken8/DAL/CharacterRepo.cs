using System;
using System.Collections.Generic;
using LogicLayer;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class CharacterRepo : ICharacterRepo
    {           
        public List<Character> GetAllCharacters()
        {
            List<Character> CharacterList = new List <Character>();

            string connectionString = ("Server=mssqlstud.fhict.local;" +
                                "Database=dbi439179_tekken;" +
                                "User Id=dbi439179_tekken;" +
                                "Password=TKDB; " +
                                "TrustServerCertificate = true");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand sqlcommand = new SqlCommand("SELECT * FROM CharacterFrameData ", connection))
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
            string connectionString = ("Server=mssqlstud.fhict.local;" +
                                "Database=dbi439179_tekken;" + 
                                "User Id=dbi439179_tekken;" +
                                "Password=TKDB; " +
                                "TrustServerCertificate = true");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand sqlcommand = new SqlCommand("SELECT * FROM CharacterFrameData WHERE Id = @Id"))
                {
                    using (SqlDataReader reader = sqlcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            character.Id = Convert.ToInt32(reader["Id"]);
                            character.Name = reader["characterName"].ToString();
                            character.EditUrl = reader["editUrl"].ToString();
                            character.Game = reader["game"].ToString();

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

