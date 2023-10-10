using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2910_TriviaGame
{
    /// <summary>
    /// Manages prompting user for quiz config information, calling QuestionBank to present answers, and keeping score of user's performance on quiz.
    /// </summary>
    internal class GameManager
    {
        private int score;
        private int maxScore;
        public int UserScore { get =>  score; set => score = value; }

        private API_QuizFetcher qFetcher { get; set; } = new API_QuizFetcher();
        private Dictionary<int, API_Category> catAlog = new Dictionary<int, API_Category>();
        private QuestionBank quiz;

        public async Task RequestCategoryInfoAsync()
        {
            Console.WriteLine("Collecting category info...");
            var temp = await qFetcher.GetCategoryInfo();
            
            // TODO: ToDictionary to get incremental integer associated with a category?
            for (int i = 1; i < temp.Count; i++) 
            {
                catAlog.Add(i, temp[i]);
            }
            catAlog.Add(catAlog.Count + 1, new API_Category(-1, "Variable"));
            Console.WriteLine($"Category count: {catAlog.Count}");
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task MenuQuizSetup()
        {
            bool bConfigSet = false;
            // // args[] = {amount, token, category, difficulty, format}
            string[] args = { "10", "token", "-1", "difficulty", "format" };

            // Category Selection
            for (int i = 1; i <= catAlog.Count; i++) 
            {
                Console.WriteLine($"{i}.\t{catAlog[i].name}");
            }
            Console.WriteLine("Please enter a number to select the trivia topic.");

            while (!bConfigSet)
            {
                string input = Console.ReadLine().Trim();
                try
                {
                    int selection = Convert.ToInt32(input);
                    if (!catAlog.ContainsKey(selection))
                    {
                        Console.WriteLine("Invalid input: please try again.");
                    }
                    // TODO : Deserialize id into string instead of int
                    args[2] = catAlog[selection].id.ToString();
                    bConfigSet = true;
                }
                catch
                {
                    Console.WriteLine("Input not recognized: please try again.");
                }
            }

            // Difficulty Selection
            Console.Clear();
            bConfigSet = false;

            Console.WriteLine("Please enter desired difficulty using the numbers below.");
            Console.WriteLine("1.\tEasy\n2.\tMedium\n3.\tHard\n4.\tVariable");

            while (!bConfigSet)
            {
                
                string input = Console.ReadLine().Trim().ToLower();

                if (input != null && QuizConfig.Difficulty.ContainsKey(input))
                {
                    args[3] = QuizConfig.Difficulty[input];
                    bConfigSet = true;
                }
                else
                {
                    Console.WriteLine("Invalid input: please try again.");
                }
            }

            // Format Selection
            Console.Clear();
            bConfigSet = false;

            Console.WriteLine("Please enter desired question format using the numbers below.");
            Console.WriteLine("1.\tTrue/False\n2.\tMultiple Choice\n3.\tVariable");

            while (!bConfigSet)
            {

                string input = Console.ReadLine().Trim().ToLower();

                if (input != null && QuizConfig.QFormat.ContainsKey(input))
                {
                    args[4] = QuizConfig.QFormat[input];
                    bConfigSet = true;
                }
                else
                {
                    Console.WriteLine("Invalid input: please try again.");
                }
            }

            // Question Count
            Console.Clear();
            bConfigSet = false;

            Console.WriteLine("Please enter how many questions you wish to appear on the quiz [5-50].");

            while (!bConfigSet)
            {

                string input = Console.ReadLine().Trim().ToLower();

                if (int.TryParse(input, out int count) && count >= 5 && count <= 50)
                {
                    args[0] = input;
                    bConfigSet = true;
                }
                else
                {
                    Console.WriteLine("Invalid input: please try again.");
                }
            }

            bConfigSet = false;
            await RequestQuestions(args);
            Console.Clear();
        }

        public async Task RequestQuestions(string[] args)
        {
            quiz = await qFetcher.GetQuestionBank(args);
        }

        public void DoQuizQuestion()
        {
            foreach (QuestionBank.Result q in quiz.results)
            {
                int ptsVal = 1 * QuizConfig.ScoreMult[q.difficulty];
                maxScore += ptsVal;
                if (quiz.AskQuestion(q))
                {
                    score += ptsVal;
                }
                Console.Clear();
            }
        }

        public void FinishQuiz()
        {
            Console.WriteLine("You have finished the quiz. You may view your score below.");
            Console.WriteLine($"You earned {score} / {maxScore} points.");
            // TODO: calculation of score percentage returning strange output.
            Console.WriteLine($"{string.Format("{0:0.##}", ((float)score / maxScore) * 100f)}%");
        }
    }
}
