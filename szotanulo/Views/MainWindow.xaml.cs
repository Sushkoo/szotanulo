using szotanulo.Data;
using szotanulo.Services;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Server=localhost;Database=szotanulo;Uid=root;Pwd=YourPassword;";
        var dbHelper = new DatabaseHelper(connectionString);
        var quizService = new QuizService(dbHelper);

        // Kapcsolódás tesztelése
        dbHelper.TestConnection();

        // Kvíz indítása
        quizService.StartQuiz();

        // Példa válasz beküldése
        Console.WriteLine("Írd be egy kérdés szavát:");
        string word = Console.ReadLine();

        Console.WriteLine("Add meg a választ:");
        string answer = Console.ReadLine();

        bool isCorrect = quizService.SubmitAnswer(word, answer);
        Console.WriteLine(isCorrect ? "Gratulálok!" : "Próbáld újra.");
    }
}
