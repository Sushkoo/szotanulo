using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using MySqlConnector;

namespace szotanulo.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Kapcsolat ellenőrzése
        /// </summary>
        public void TestConnection()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Sikeresen csatlakozott az adatbázishoz!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Adatbázis kapcsolódási hiba: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Szavak lekérdezése az adatbázisból
        /// </summary>
        public List<(string Word, string Meaning)> GetWords()
        {
            var words = new List<(string Word, string Meaning)>();
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT Word, Meaning FROM Words WHERE Learned = 0;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var word = reader.GetString("Word");
                                var meaning = reader.GetString("Meaning");
                                words.Add((word, meaning));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a szavak lekérdezésekor: {ex.Message}");
                throw;
            }

            return words;
        }

        /// <summary>
        /// Új szó beszúrása az adatbázisba
        /// </summary>
        public void InsertWord(string word, string meaning)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO Words (Word, Meaning, Learned) VALUES (@Word, @Meaning, 0);";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Word", word);
                        command.Parameters.AddWithValue("@Meaning", meaning);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba az új szó beszúrásakor: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Szó megtanultként jelölése
        /// </summary>
        public void MarkWordAsLearned(string word)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE Words SET Learned = 1 WHERE Word = @Word;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Word", word);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a szó megtanultként jelölésekor: {ex.Message}");
                throw;
            }
        }
    }
}
