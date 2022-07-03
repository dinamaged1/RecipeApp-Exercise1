using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Exercise1;
using System.Text.Json;
using Spectre.Console;

public class Program
{
    public static async Task Main(string[] args)
    {

        //Desrialize recipe file and category file
        string recipeJson =await ReadJsonFile("recipe");
        string categoryJson =await ReadJsonFile("category");
        var savedRecipes = JsonSerializer.Deserialize<List<Recipe>>(recipeJson);
        var savedCategories = JsonSerializer.Deserialize<List<string>>(categoryJson);

        //Create list of recipes and list of categories
        List<Recipe> recipesList = new List<Recipe>(savedRecipes!);
        List<string> categoryList = new List<string>(savedCategories!);

        //Adding console GUI
        AnsiConsole.Write(
        new FigletText("Recipe App")
        .Centered()
        .Color(Color.Fuchsia));
        string[] choices = new string[] { "Recipe", "Category", "Exit" };

        while (true)
        {
            choices = new string[] { "Recipe", "Category", "Exit" };
            string firstMenuChoice = ConsoleSelection(choices);
            var secondMenuChoice = "";
            switch (firstMenuChoice)
            {
                case "Recipe":
                    choices = new string[] { "Add recipe", "Edit recipe", "List recipes", "Exit" };
                    secondMenuChoice = ConsoleSelection(choices);
                    break;

                case "Category":
                    choices = new string[] { "Add Category", "Edit Category", "Exit" };
                    secondMenuChoice = ConsoleSelection(choices);

                    break;

                case "Exit":
                    return;

                default:
                    break;
            }

            switch (secondMenuChoice)
            {
                case "Add recipe":
                    bool addingStatus = await AddRecipe(categoryList, recipesList);
                    break;

                case "List recipes":
                    ListRecipes(recipesList);
                    break;

                case "Exit":
                    return;

                default:
                    break;
            }
        }
    }

    public static async Task<string> ReadJsonFile(string fileName) =>
    await File.ReadAllTextAsync($"{fileName}.json");

    public static async Task WriteJsonFile(string fileName, string fileData) =>
    await File.WriteAllTextAsync($"{fileName}.json", fileData);

    public static async Task<bool> AddRecipe(List<string> categoryList, List<Recipe> recipesList)
    {
        //Get the data of the recipe from the user
        string title = AnsiConsole.Ask<string>("What's recipe name?");
        string instructions = AnsiConsole.Ask<string>("What's recipe instructions? (ex: milk-sugar-cocoa powder)");
        string ingerdiants = AnsiConsole.Ask<string>("What's recipe ingerdiants (ex: Pour in the Milk-Add cocoa powder-Add sugar)?");
        var categories = ConsoleMultiSelection(categoryList);
        List<string> ingerdiandsList = instructions.Split('-').ToList();
        List<string> instructionsList = ingerdiants.Split('-').ToList();
        Recipe newRecipe = new Recipe(title, instructionsList, ingerdiandsList, categories);
        recipesList.Add(newRecipe);

        //Serialize recipe list and add it to recipe file
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(recipesList, options);
        await WriteJsonFile("recipe", jsonString);
        return true;
    }

    public static void ListRecipes(List<Recipe> recipesList)
    {
        //create table to view all recipes
        var recipeTable = new Table();
        recipeTable.AddColumn("Recipe Title");
        recipeTable.AddColumn("Ingrediants");
        recipeTable.AddColumn("Instructions");
        recipeTable.AddColumn("Category");
        recipeTable.Border(TableBorder.Rounded);
        recipeTable.Centered();

        foreach (Recipe recipe in recipesList)
        {
            recipeTable.AddRow($"[yellow]{recipe.Title}[/]", " -" + string.Join("\n -", recipe.Ingerdiants), " -" + string.Join("\n -", recipe.Instructions), " -" + string.Join("\n -", recipe.Categories));
            recipeTable.AddRow("-------------", "------------", "-------------------", "----------------");
        }

        AnsiConsole.Write(recipeTable);
    }

    public static bool AddCategory()
    {
        //TODO
        return true;
    }

    public static string ConsoleSelection(string[] list)
    {
        var action = "";

        //Show user avaliable actions
        action = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
               .Title("How can I serve you?")
               .PageSize(10)
               .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
               .AddChoices(list)
    );
        return action;
    }

    public static List<string> ConsoleMultiSelection(List<string> categoryList)
    {
        var selectedCategories = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
               .Title("What's recipe categories?")
               .NotRequired()
               .PageSize(10)
               .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
               .InstructionsText(
                   "[grey](Press [blue]<space>[/] to toggle a category, " +
                   "[green]<enter>[/] to accept)[/]")
               .AddChoices(categoryList));

        return selectedCategories;
    }
}