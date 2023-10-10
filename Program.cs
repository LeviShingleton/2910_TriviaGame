namespace _2910_TriviaGame
{
    internal class Program
    {
        static async Task Main()
        {
            GameManager manager = new GameManager();
            // Get category info
            await manager.RequestCategoryInfoAsync();
            // Create menu from category info
            // Accept input to drive category selection
            // Get selection questions
            await manager.MenuQuizSetup();
            // Present questions and accept answers
            // Grade on-the-fly
            manager.DoQuizQuestion();
            // Present performance, option to repeat
            manager.FinishQuiz();
        }
    }
}