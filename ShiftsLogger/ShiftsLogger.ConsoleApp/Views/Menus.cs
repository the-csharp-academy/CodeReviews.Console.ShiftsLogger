using ShiftsLogger.ConsoleApp.Controllers;
using ShiftsLogger.ConsoleApp.Services;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Views;

internal class Menus
{
    private readonly ShiftService _shiftService;

    private readonly WorkerService _workerService;

    public Menus(ShiftService shiftService, WorkerService workerService)
    {
        _shiftService = shiftService;
        _workerService = workerService;
    }


    public static async Task PrintSelectionMainMenu()
    {
        var isAppRunning = true;
        while (isAppRunning)
        {
            Console.Clear();
            var options = new[] { "View & Manage Shifts", "View & Manage Workers", "Close Shifts Logger" };

            var mainMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("MAIN MENU")
                .PageSize(10)
                .AddChoices(options));

            var menu = new Menus(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));

            switch (mainMenuChoice)
            {
                case "View & Manage Shifts":
                    await menu.PrintSelectionShiftsMenu();
                    break;
                case "View & Manage Workers":
                    await menu.PrintSelectionWorkersMenu();
                    break;
                case "Close Shifts Logger":
                    Console.WriteLine("Goodbye!");
                    Thread.Sleep(2000);
                    isAppRunning = false;
                    break;
            }
        }

    }


    internal async Task PrintSelectionWorkersMenu()
    {
        var isWorkersMenuRunning = true;
        while (isWorkersMenuRunning)
        {
            Console.Clear();

            var options = new[] { "View Workers", "Add Worker", "Edit Worker", "Delete Worker", "Return to Main Menu" };

            var workersMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("WORKERS MENU")
                .PageSize(10)
                .AddChoices(options));

            switch (workersMenuChoice)
            {
                case "View Workers":
                    await WorkerController.ViewWorkers("View Workers");
                    ReturnToPreviousMenu();
                    break;
                case "Add Worker":
                    await WorkerController.CreateWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Edit Worker":
                    await WorkerController.EditWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Delete Worker":
                    await WorkerController.DeleteWorker();
                    ReturnToPreviousMenu();
                    break;
                case "Return to Main Menu":
                    isWorkersMenuRunning = false;
                    break;
            }
        }
    }


    internal async Task PrintSelectionShiftsMenu()
    {
        var isShiftsMenuRunning = true;
        while (isShiftsMenuRunning)
        {
            Console.Clear();

            var options = new[] { "View Shifts", "Add Shift", "Edit Shift", "Delete Shift", "Return to Main Menu" };

            var shiftsMenuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("SHIFTS MENU")
                .PageSize(10)
                .AddChoices(options));

            switch (shiftsMenuChoice)
            {
                case "View Shifts":
                    await ShiftController.ViewShifts("View Shifts");
                    ReturnToPreviousMenu();
                    break;
                case "Add Shift":
                    await ShiftController.CreateShift();
                    ReturnToPreviousMenu();
                    break;
                case "Edit Shift":
                    await ShiftController.EditShift();
                    ReturnToPreviousMenu();
                    break;
                case "Delete Shift":
                    await ShiftController.DeleteShift();
                    ReturnToPreviousMenu();
                    break;
                case "Return to Main Menu":
                    isShiftsMenuRunning = false;
                    break;
            }
        }
    }

    public static void ReturnToPreviousMenu()
    {
        Console.Write("\nPress any key to return to the previous menu...");
        Console.ReadKey();
    }
}
