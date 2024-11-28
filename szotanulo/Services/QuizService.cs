using System;
using System.Collections.Generic;
using System.Linq;
using szotanulo.Data;

namespace szotanulo.Services
{
    public class QuizService
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly Random _random;
        private readonly List<(string Word, string Meaning)> _currentQuestions;

        public QuizService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
            _random = new Random();
            _currentQuestions = new List<(string Word, string Meaning)>();
        }

        /// <summary>
        /// Kvíz indítása, 10 véletlenszerű szóval.
        /// </summary>
        public void StartQuiz()
        {
            var allWords = _databaseHelper.GetWords();

            if (allWords.Count < 10)
            {
                Console.WriteLine("Nem áll rendelkezésre elég szó a kvízhez.");
                return;
            }

            _currentQuestions.Clear();
            _currentQuestions.AddRange(allWords.OrderBy(_ => _random.Next()).Take(10));

            Console.WriteLine("A kvíz elkezdődött! Válaszolj a következő kérdésekre:");
            foreach (var question in _currentQuestions)
            {
                Console.WriteLine($"Mit jelent: {question.Word}?");
            }
        }

        /// <summary>
        /// Egy kérdés megválaszolása.
        /// </summary>
        public bool SubmitAnswer(string word, string userAnswer)
        {
            var correctAnswer = _currentQuestions.FirstOrDefault(q => q.Word == word);

            if (correctAnswer == default)
            {
                Console.WriteLine("A kérdés nem található a jelenlegi kvízben.");
                return false;
            }

            if (string.Equals(correctAnswer.Meaning, userAnswer, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Helyes válasz!");
                MarkWordAsLearned(word);
                return true;
            }
            else
            {
                Console.WriteLine($"Helytelen! A helyes válasz: {correctAnswer.Meaning}");
                return false;
            }
        }

        /// <summary>
        /// Egy szó megtanultként jelölése.
        /// </summary>
        private void MarkWordAsLearned(string word)
        {
            _databaseHelper.MarkWordAsLearned(word);
        }
    }
}