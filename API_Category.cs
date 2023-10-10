using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2910_TriviaGame
{
    internal class API_Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public API_Category()
        {

        }
        public API_Category(int ID, string Name)
        {
            id = ID;
            name = Name;
        }
    }
}
