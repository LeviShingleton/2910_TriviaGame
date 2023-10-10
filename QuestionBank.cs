using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _2910_TriviaGame
{
    internal class QuestionBank
    {
        public int response_code { get; set; }
        public Result[] results { get; set; }
        private Random rng = new Random();
        public class ResponseInfo
        {
            public int response_code { get; set; }
            public Result[] questions { get; set; }
        }

        public class Result
        {
            public string category { get; set; }
            public string type { get; set; }
            public string difficulty { get; set; }
            public string question { get; set; }
            public string correct_answer { get; set; }
            public string[] incorrect_answers { get; set; }
        }
        public bool AskQuestion(Result q)
        {
            int value = 1 * QuizConfig.ScoreMult[q.difficulty];
            StringBuilder s = new StringBuilder($"({q.category}) [{value} pts]" +
                $"\n{HttpUtility.HtmlDecode(q.question)}\n\n");
            
            if (q.type.Equals("boolean"))
            {
                s.Append("True\nFalse");
            }
            else if (q.type.Equals("multiple"))
            {
                string[] responses = new string[4];
                q.incorrect_answers.CopyTo(responses, 0);
                responses[3] = q.correct_answer;
                Shuffle(responses);

                foreach (string response in responses) { s.Append($"{HttpUtility.HtmlDecode(response)}\n"); }
            }

            Console.WriteLine($"{s}\n");
            Console.WriteLine("Please enter your answer as listed above. You may copy-paste.");
            Console.Write("Your answer: \t");
            string input = Console.ReadLine().Trim().ToLower();

            while (input.Equals(""))
            {
                Console.WriteLine("Please enter a response.");
                input = Console.ReadLine();
            }

            if (input.Equals(HttpUtility.HtmlDecode(q.correct_answer).ToLower()))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Shuffles responses array using Knuth Shuffle
        /// https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rng"></param>
        /// <param name="array"></param>
        public void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
