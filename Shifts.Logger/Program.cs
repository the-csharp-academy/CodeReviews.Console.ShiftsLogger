using Spectre.Console;

namespace Shifts.Logger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Choose action")
                    .AddChoices(
                        "Get all shifts",
                        "Create shift",
                        "Update shift",
                        "Delete shift",
                        "Exit"));

                    if (choice == "Exit")
                        break;

                    else if (choice == "Get all shifts")

                        await UI.PrintTable();

                    else if (choice == "Create shift")
                    {
                        var result = await UI.CreateShift();
                        if (result)
                            AnsiConsole.MarkupLine("Shift succesfully added");
                        else
                            AnsiConsole.MarkupLine("Error when add shift");
                    }

                    else if (choice == "Update shift")
                    {
                        await UI.PrintTable();
                        var result = await UI.UpdateShift();
                        if (result)
                            AnsiConsole.MarkupLine("Shift succesfully updated");
                        else
                            AnsiConsole.MarkupLine("Error when updated shift");
                    }

                    else if (choice == "Delete shift")
                    {
                        await UI.PrintTable();
                        var result = await UI.DeleteShift();
                        if (result)
                            AnsiConsole.MarkupLine("Shift succesfully deleted");               
                        else
                            AnsiConsole.MarkupLine("Error when deleted shift");      
                    }
                    AnsiConsole.MarkupLine("Press any key...");
                    Console.ReadKey(true);
                    AnsiConsole.Clear();
                }

                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"Error: {ex.Message}");
                }
            }
        }
    }
}