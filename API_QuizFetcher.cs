using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2910_TriviaGame
{
    // Open Trivia Database Doc:https://opentdb.com/api_config.php
    /* API NOTES
     * Request parameters: Amount, token, category, difficulty, format, encoding
     * CATEGORY LOOKUP REQUEST: https://opentdb.com/api_category.php => Used to generate dictionary of categories
     * 
     * API LIMITATIONS
     * Only 1 category request can be made per call. In this implementation, category scope is determined before call(s) are made.
     * Only 50 questions may be retrieved in one API call.
     * 
     * TODO: Implement token to prevent repeat questions
     */
    internal class API_QuizFetcher
    {
        HttpClient client = new HttpClient();
        public API_QuizFetcher()
        {
            client = new HttpClient();
        }


        /// <summary>
        /// Mini-class created by Edit > Paste Special > Paste JSON as class
        /// Used to facilitate hard type deserialization
        /// </summary>
        public class API_JsonCategories
        {
            public API_Category[] trivia_categories { get; set; }
        }

        /// <summary>
        /// Returns a list of current OTB categories via API call.
        /// Called by GameManager to be stored in local dictionary.
        /// </summary>
        /// <returns></returns>
        public async Task<List<API_Category>> GetCategoryInfo()
        {
            // Get category JSON
            HttpResponseMessage response = await client.GetAsync($"https://opentdb.com/api_category.php");
            string json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON into List<API_Category> via NewtonSoft
            API_JsonCategories temp = JsonConvert.DeserializeObject<API_JsonCategories>(json);

            return temp.trivia_categories.ToList();
        }


        //public class test_QB
        //{
        //    public int response_code { get; set; }
        //    public Result[] results { get; set; }
        //}

        //public class Result
        //{
        //    public string category { get; set; }
        //    public string type { get; set; }
        //    public string difficulty { get; set; }
        //    public string question { get; set; }
        //    public string correct_answer { get; set; }
        //    public string[] incorrect_answers { get; set; }
        //}


        /// <summary>
        /// Retrieves a question bank response from API using string[] args
        /// </summary>
        /// <param name="args">{amount, token, category, difficulty, format} | -1 represents a default/ignored parameter.</param>
        /// <returns></returns>
        public async Task<QuestionBank> GetQuestionBank(string[] args)
        {
            //https://opentdb.com/api.php?amount=10&category=9&difficulty=easy&type=multiple

            // args[] = {amount, token, category, difficulty, format}

            StringBuilder urlAssembler = new StringBuilder($"https://opentdb.com/api.php?amount={args[0]}");

            if (args[1] != "token")
            {
                urlAssembler.Append($"&token={args[1]}");
            }
            if (!args[2].Equals("-1"))
            {
                urlAssembler.Append($"&category={args[2]}");
            }
            if (!args[3].Equals("difficulty"))
            {
                urlAssembler.Append($"&difficulty={args[3]}");
            }
            if (!args[4].Equals("format"))
            {
                urlAssembler.Append($"&type={args[4]}");
            }

            HttpResponseMessage response = await client.GetAsync(urlAssembler.ToString());
            string json = await response.Content.ReadAsStringAsync();

            QuestionBank qb = JsonConvert.DeserializeObject<QuestionBank>(json);

            return JsonConvert.DeserializeObject<QuestionBank>(json);
        }
    }
}
