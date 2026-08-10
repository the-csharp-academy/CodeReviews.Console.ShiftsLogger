using ShiftLogger.UI.DTOs;
using ShiftLogger.UI.Enums;
using ShiftLogger.UI.Extensions;
using ShiftLogger.UI.Menus;
using ShiftLogger.UI.Model;
using ShiftLogger.UI.Services;
using Spectre.Console;

namespace ShiftLogger.UI.MenuOptions;

internal class ShiftManager
{
    private readonly WorkerShiftService workerShiftService = new();
    private readonly UserInputs userInputs = new();

    internal async Task ViewAll()
    {
        Console.Clear();

        List<Shift>? result = await workerShiftService.GetAllWorkerShifts();

        if (result == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error displaying all work shifts[/]");
        }
        else
        {
            TableGenerator.ShowTable(result, "Worker shifts Info");
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();

        return;
    }

    internal async Task ViewWorkerShiftById()
    {
        Console.Clear();

        Shift? selectedShift = await GetOptionWorkerShift("Select a shift to view");

        if (selectedShift == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error displaying all work shifts[/]");
        }
        else
        {
            Shift? result = await workerShiftService.GetWorkerShiftById(selectedShift.ShiftId.ToString());

            if (result == null)
            {
                AnsiConsole.MarkupLine("[#F0362C]Error found worker shift[/]");
            }
            else
            {
                var table = TableGenerator.CreateDetailsTable(result, "Worker Shifts Details");
                AnsiConsole.Write(table);
            }
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();

        return;
    }

    internal async Task CreateWorkerShift()
    {
        Console.Clear();

        CreateWorkerShiftDTO dto = new();


        dto.WorkerName = userInputs.GetWorkerName();
        dto.Date = userInputs.GetDateShift();
        dto.StartTime = userInputs.GetStartTime();
        dto.EndTime = userInputs.GetEndTime(dto.StartTime);
        dto.ShiftTime = userInputs.GetShiftTime();


        Shift? result = await workerShiftService.CreateWorkerShift(dto);

        if (result == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error craeting a new worker shift[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[#008148]Worker shift craeted successfully[/]");
            var table = TableGenerator.CreateDetailsTable(result, "New Worker Shift");
            AnsiConsole.Write(table);
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();
    }

    internal async Task UpdateWorkerShift()
    {
        Console.Clear();

        Shift? selectedShift = await GetOptionWorkerShift("Select a shift to update");

        if (selectedShift == null)
            return;

        UpdateWorkerShiftDTO dto = new();

        bool updating = true;

        while (updating)
        {
            Console.Clear();

            AnsiConsole.Write(TableGenerator.CreateUpdateView(selectedShift));

            UpdateOptions option = AnsiConsole.Prompt(
        new SelectionPrompt<UpdateOptions>()
            .Title("Select the field to update")
            .UseConverter(option => option.ToDisplayString())
            .AddChoices(Enum.GetValues<UpdateOptions>()));

            switch (option)
            {
                case UpdateOptions.WorkerName:
                    dto.WorkerName = userInputs.GetWorkerName();
                    break;
                case UpdateOptions.Date:
                    dto.Date = userInputs.GetDateShift();
                    break;
                case UpdateOptions.StartTime:
                    dto.StartTime = userInputs.GetStartTime();
                    break;
                case UpdateOptions.EndTime:
                    dto.EndTime = userInputs.GetEndTime(dto.StartTime == null
                        ? selectedShift.StartTime
                        : (TimeOnly)dto.StartTime);
                    break;
                case UpdateOptions.ShiftTime:
                    dto.ShiftTime = userInputs.GetShiftTime();
                    break;
                case UpdateOptions.SaveChanges:
                    updating = false;
                    break;
                case UpdateOptions.Cancel:
                    return;
            }

        }

        Shift? result = await workerShiftService.UpdateWorkerShift(selectedShift.ShiftId, dto);

        if (result == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error updating a worker shift[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[#008148]Worker shift updated successfully[/]");
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();
    }
    internal async Task DeleteWorkerShift()
    {
        Console.Clear();

        Shift? selectedShift = await GetOptionWorkerShift("Select a shift to Delete");

        if (selectedShift == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error displaying all work shifts[/]");
        }
        else
        {
            string? result = await workerShiftService.DeleteWorkerShift(selectedShift.ShiftId);

            if (result == null)
            {
                AnsiConsole.MarkupLine("[#F0362C]Error deleting a worker shift[/]");
            }
            else
            {
                var table = TableGenerator.CreateDetailsTable(selectedShift, "Deleted Shift");
                AnsiConsole.Write(table);
                AnsiConsole.MarkupLine($"[#008148]{result}[/]");
            }
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();
    }

    private async Task<Shift?> GetOptionWorkerShift(string message)
    {
        List<Shift>? workerShifts = await workerShiftService.GetAllWorkerShifts();

        if (workerShifts == null)
        {
            return null;
        }

        Shift selectedShift = AnsiConsole.Prompt(
            new SelectionPrompt<Shift>()
            .Title($"{message}")
            .UseConverter(shift => $"{shift.WorkerName} | [#558B6E]({shift.Date:yyyy-MM-dd})[/] | {shift.ShiftTime}")
            .AddChoices(workerShifts));

        return selectedShift;
    }
}