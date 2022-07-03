using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Exercise1;
using System.Text.Json;
using System.Reflection;
using Spectre.Console;
using System.Threading.Tasks;


public class Program
{
    public static async Task Main(string[] args)
    {
        //Desrialize recipe file and category file
        string recipeJson = ReadJsonFile("recipe").Result;
        string categoryJson = ReadJsonFile("category").Result;
        var savedRecipes = JsonSerializer.Deserialize<List<Recipe>>(recipeJson);
        var savedCategories = JsonSerializer.Deserialize<List<string>>(categoryJson);

        //Create list of recipes and list of categories
        List<Recipe> recipesList = new List<Recipe>(savedRecipes!);
        List<string> categoryList=new List<string>(savedCategories!);
        Console.WriteLine();

        //Adding console GUI
        AnsiConsole.Write(
        new FigletText("Recipe App")
        .Centered()
        .Color(Color.Fuchsia));
        string[] actions = new string[] { "Recipe", "Category", "Exit" };

        while (true)
        {
            string action1 = ConsoleSelection(actions);
            var action2 = "";
            switch (action1)
            {
                case "Recipe":
                    actions = new string[] { "Add recipe", "Edit recipe", "List recipes", "Exit" };
                    action2 = ConsoleSelection(actions);
                    break;

                case "Category":
                    actions = new string[] { "Add Category", "Edit Category", "Exit" };
                    action2 = ConsoleSelection(actions);

                    break;

                case "Exit":
                    return;

                default:
                    break;
            }

            switch (action2)
            {
                case "Add recipe":
                    bool addingStatus= AddRecipe(categoryList,ref recipesList);
                    break;

                case "List recipes":
                    ListRecipes(recipesList);
                    break;

                default:
                    break;
            }
        }
    }

    public static async Task<string> ReadJsonFile(string fileName) =>
    await File.ReadAllTextAsync($"{fileName}.json");

    public static bool AddRecipe(List<string> categoryList,ref List<Recipe> recipesList) {

        string title = AnsiConsole.Ask<string>("What's recipe name?");
        string instructions = AnsiConsole.Ask<string>("What's recipe instructions? (ex: milk-water-chocolate)");
        string ingerdiants= AnsiConsole.Ask<string>("What's recipe ingerdiants (ex: Boil the milk-Put all ingrediants together)?");
        var categories = SelectCategories(categoryList);
        List<string> ingerdiandsList = instructions.Split('-').ToList();
        List<string> instructionsList= ingerdiants.Split('-').ToList();
        Recipe newRecipe=new Recipe(title, instructionsList, ingerdiandsList, categories);
        recipesList.Add(newRecipe);
        return true;

    }

    public static void ListRecipes(List<Recipe> recipesList)
    {
        foreach (Recipe recipe in recipesList)
        {
            
        }
    }
    public static bool AddCategory(){
        return true;
    }
    public static string ConsoleSelection(string[] list)
    {
            var action = "";

            //Show user avaliable actions
            action = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                   .Title("What change you want ?")
                   .PageSize(10)
                   .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                   .AddChoices(list)
        );
            return action;
    }

    public static List<string> SelectCategories(List<string> categoryList)
    {
        var selectedCategories = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
               .Title("What's recipe categories [green]favorite fruits[/]?")
               .NotRequired() // Not required to have a favorite fruit
               .PageSize(10)
               .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
               .InstructionsText(
                   "[grey](Press [blue]<space>[/] to toggle a category, " +
                   "[green]<enter>[/] to accept)[/]")
               .AddChoices(categoryList));

        return selectedCategories;

    }
}
