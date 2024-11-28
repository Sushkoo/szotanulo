using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using szotanulo.Data;

namespace szotanulo.Services
{
    public class CsvImporter
    {
        private readonly string _connectionString;

        public CsvImporter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            var lines = File.ReadAllLines(filePath);
            var words = new List<Word>();

            foreach (var line in lines)
            {
                var parts = line.Split(';'); // CSV fájl ";" karakterrel elválasztva
                if (parts.Length != 2) continue;

                words.Add(new Word
                {
                    WordText = parts[0].Trim(),
                    Meaning = parts[1].Trim(),
                    Learned = 0
                });
            }

            SaveWordsToDatabase(words);
        }

        private void SaveWordsToDatabase(List<Word> words)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var word in words)
                {
                    var query = "INSERT INTO Words (Word, Meaning, Learned) VALUES (@Word, @Meaning, @Learned)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Word", word.WordText);
                        command.Parameters.AddWithValue("@Meaning", word.Meaning);
                        command.Parameters.AddWithValue("@Learned", word.Learned);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
