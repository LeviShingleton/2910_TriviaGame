using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2910_TriviaGame
{
    static class QuizConfig
    {
        static public Dictionary<string, string> Difficulty = new Dictionary<string, string>() { { "1", "easy"}, { "easy", "easy" },
            { "2", "medium" }, { "medium", "medium" },
            { "3", "hard" }, { "hard","hard" },
            { "4", "difficulty"}, { "variable", "difficulty"} };

        public static Dictionary<string, string> QFormat = new Dictionary<string, string>() { { "1", "boolean" }, { "2", "multiple" }, { "3", "format" } };

        public static Dictionary<string, int> ScoreMult = new Dictionary<string, int>() { { "easy", 1 }, { "medium", 2 }, { "hard", 3 } };
    }
}
