using ShiftsLoggerUI.Services;
using ShiftsLoggerUI.Controllers;
using Spectre.Console;

namespace ShiftsLoggerUI.Menus
{
    internal class UserInterface
    {



        internal static async Task MainMenu()
        {
            bool running = true;

            while (running)
            {

                Console.Clear();
                AnsiConsole.MarkupLine("[slowblink][red] Welcome to the shift logging main menu[/][/]");

                Console.WriteLine();

                var choice = AnsiConsole.Prompt(
                   new SelectionPrompt<string>()
                   .Title("Please select an option:")
                   .AddChoices(new[]
                   {
                    "Log a new shift",
                    "View all past shifts",
                    "Update a shift",
                    "Delete a shift",
                    "Exit"
                   }));

                
                switch (choice)
                {
                    case "Log a new shift":
                    {
                        var shiftController = new ShiftController();
                        await shiftController.LogShift();
                        break;
                    }

                    case "View all past shifts":
                    {
                        var shiftsService = new ShiftsService();
                        await shiftsService.ShowAllShifts();
                        break;
                    }

                    case "Update a shift":
                    {
                        var shiftController = new ShiftController();
                        await shiftController.UpdateShift();
                        break;
                    }

                    case "Delete a shift":
                    {
                        var shiftController = new ShiftController();
                        await shiftController.DeleteShift();
                        break;
                    }

                    case "Exit":
                        AnsiConsole.MarkupLine("[slowblink][red] Exiting the application. Goodbye![/][/]");
                        Environment.Exit(0);
                        break;
                }

            }

        }
    }
}
