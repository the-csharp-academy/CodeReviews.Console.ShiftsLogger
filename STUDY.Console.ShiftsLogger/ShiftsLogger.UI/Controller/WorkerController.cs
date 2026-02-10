using System.Globalization;
using ShiftsLogger.UI.DTOs;
using ShiftsLogger.UI.Services;
using Spectre.Console;

namespace ShiftsLogger.UI.Controller;

public class WorkerController
{
    private readonly WorkerService _workerService = new();

    public async Task AddWorker()
    {
        var name = AnsiConsole.Ask<string>("Enter worker name: ");

        try
        {
            await _workerService.AddWorker(name);
            AnsiConsole.MarkupLine("[green]Worker added successfully![/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
        }
    }

    public async Task<WorkerDto?> GetAllWorkers()
    {
        try
        {
            var workers = await _workerService.GetAllWorkers();
            if (workers is null)
            {
                AnsiConsole.MarkupLine("[red]No workers found![/]");
                return null;
            }

            var workerChoice = AnsiConsole.Prompt<WorkerDto>(
                new SelectionPrompt<WorkerDto>()
                    .Title("Select A Worker to View Shifts:")
                    .AddChoices(workers)
                    .UseConverter(w => w.Name)
                    .EnableSearch()
            );
            return workerChoice;
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            return null;
        }
    }

    //==========================

    public async Task<string?> UpdateWorker(WorkerDto workerDto)
    {
        var newName =
            AnsiConsole.Ask<string>("Enter new worker name - or press ENTER to keep old name", workerDto.Name);
        if (newName == workerDto.Name)
        {
            return null;
        }

        try
        {
            var workerIsUpdated = await _workerService.UpdateWorker(workerDto.Id, newName);


            if (workerIsUpdated)
            {
                AnsiConsole.MarkupLine("[green]Worker updated successfully![/]");
                return newName;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Worker not updated![/]");
                return null;
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            return null;
        }
    }

    public async Task<bool> DeleteWorker(WorkerDto workerDto)
    {
        var workerIsDeleted = await _workerService.DeleteWorker(workerDto.Id);
        AnsiConsole.MarkupLine(workerIsDeleted
            ? "[green]Worker deleted successfully![/]"
            : "[red]Worker not deleted![/]");
        return workerIsDeleted;
    }

    public async Task ManageWorkerShifts(WorkerDto workerDto)
    {
        try
        {
            bool inShiftMenu = true;
            while (inShiftMenu)
            {
                var workerDetails = await _workerService.GetWorkerDetails(workerDto.Id);
                if (workerDetails is null)
                {
                    AnsiConsole.MarkupLine("[red]Worker not found![/]");
                    return;
                }

                var table = new Table().AddColumns("No.", "Start Time", "End Time");
                List<string> choices = new List<string>();
                if (workerDetails.Shifts.Count == 0)
                {
                    table.AddEmptyRow();
                    table.Caption("[yellow]Worker has no shifts yet![/]");
                    choices.AddRange("Add New Shift", "Back to Main Menu");
                }
                else
                {
                    int i = 1;
                    foreach (var shift in workerDetails.Shifts)
                    {
                        table.AddRow($"{i++}", shift.StartTime.ToString(CultureInfo.InvariantCulture), shift.EndTime.ToString(CultureInfo.InvariantCulture));
                    }

                    AnsiConsole.Write(table);
                    choices.AddRange("Add New Shift", "Update Shift", "Delete Shift", "Back to Main Menu");
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Shifts Management for: [cyan]{workerDto.Name}[/]")
                        .AddChoices(choices));
                ShiftsController _shiftsController = new ShiftsController();
                switch (choice)
                {
                    case "Add New Shift":
                        await _shiftsController.AddShift(workerDto);
                        break;
                    case "Update Shift":
                        break;
                    case "Delete Shift":
                        break;
                    case "Back to Main Menu":
                        inShiftMenu = false;
                        break;
                }
                
                if (choice != "Back to Main Menu")
                {
                    AnsiConsole.MarkupLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    AnsiConsole.Clear();
                }
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
        }
    }
}