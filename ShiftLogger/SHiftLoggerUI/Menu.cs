namespace ShiftLoggerUI;

using Spectre.Console;

public class Menu
{
    public static async Task ShowMenu()
    {
        bool wantToExit = false;

        while (!wantToExit)
        {
                Console.Clear();

            var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuChoice>()
                .Title("Select Menu: ")
                .AddChoices(Enum.GetValues<MenuChoice>())
            );

            switch (choice)
            {
                case MenuChoice.Workers:
                    await ShowMenuWorkers();
                    break;

                case MenuChoice.Shifts:
                    await ShowMenuShifts();
                    break;

                case MenuChoice.Exit:
                    wantToExit = true;
                    Console.WriteLine("Now closing program.");
                    break;
            }
        }
    }

    public static async Task ShowMenuWorkers()
    {
        var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuActionsWorker>()
            .Title("Select new action: ")
            .AddChoices(Enum.GetValues<MenuActionsWorker>())
        );

        switch (choice)
        {
            case MenuActionsWorker.GetWorkers:
            await WorkerService.GetWorkers();
            break;

            case MenuActionsWorker.GetWorker:
            await WorkerService.GetWorker();
            break;

            case MenuActionsWorker.GetWorkerShifts:
            await WorkerService.GetWorkerShifts();
            break;

            case MenuActionsWorker.AddWorker:
            await WorkerService.AddWorker();
            break;

            case MenuActionsWorker.EditWorker:
            await WorkerService.EditWorker();
            break;

            case MenuActionsWorker.DeleteWorker:
            await WorkerService.DeleteWorker();
            break;

            case MenuActionsWorker.GoBack:
            break;
        }
    }

    public static async Task ShowMenuShifts()
    {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuActionsShift>()
            .Title("Select new action: ")
            .AddChoices(Enum.GetValues<MenuActionsShift>())
        );

        switch (choice)
        {
            case MenuActionsShift.GetShifts:
            await ShiftService.GetShifts();
            break;

            case MenuActionsShift.GetShift:
            await ShiftService.GetShift();
            break;

            case MenuActionsShift.AddShift:
            await ShiftService.AddShift();
            break;

            case MenuActionsShift.EditShift:
            await ShiftService.EditShift();
            break;

            case MenuActionsShift.DeleteShift:
            await ShiftService.DeleteShift();
            break;

            case MenuActionsShift.GoBack:
            break;
        }
    }
}