using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exercise1
{
    public class Recipe
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Instructions { get; set; } = new();
        public List<string> Ingerdiants { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public Recipe(string title, List<string> instructions, List<string> ingerdiants, List<string> categories)
        {
            Title = title;
            Instructions = instructions;
            Ingerdiants = ingerdiants;
            Categories = categories;
        }
        public Recipe()
        {
                
        }
    }
}
