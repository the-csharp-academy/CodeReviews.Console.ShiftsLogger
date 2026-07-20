using ShiftLogger.UI.DTOs;
using ShiftLogger.UI.Model;
using ShiftLogger.UI.Services;
using Spectre.Console;
using static ShiftLogger.UI.Menus.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShiftLogger.UI.Menus;

internal class Menu
{
    private readonly WorkerShiftService workerShiftService = new();

    internal async Task MainMenu()
    {
        bool open = true;

        while (open)
        {
            Console.Clear();

            var options = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.MainMenuOptions>()
                .Title("Select an option: ")
                .AddChoices(Enum.GetValues<Enums.MainMenuOptions>()));

            switch (options)
            {
                case Enums.MainMenuOptions.CreateWorkerShift:
                    await CreateWorkerShift();
                    break;
                case Enums.MainMenuOptions.ViewAllWorkerShift:
                    await ViewAll();
                    break;
                case Enums.MainMenuOptions.ViewWorkerShift:
                    await ViewWorkerShiftById();
                    break;
                case Enums.MainMenuOptions.UpdateWorkerShift:
                    await UpdateWorkerShift();
                    break;
                case Enums.MainMenuOptions.DeleteWorkerShift:
                    await DeleteWorkerShift();
                    break;
                case Enums.MainMenuOptions.Exit:
                    open = false;
                    break;
            }
        }
    }

    private async Task ViewAll()
    {
        Console.Clear();

        List<WorkerShift> result = await workerShiftService.GetAllWorkerShifts();

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

    private async Task ViewWorkerShiftById()
    {
        Console.Clear();

        WorkerShift selectedShift = await GetOptionWorkerShift("Select a shift to view");

        if (selectedShift == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error displaying all work shifts[/]");
        }
        else
        {
            WorkerShift result = await workerShiftService.GetWorkerShiftById(selectedShift.ShiftId.ToString());

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

    private async Task CreateWorkerShift()
    {
        Console.Clear();

        CreateWorkerShiftDTO dto = new();


        dto.WorkerName = GetWorkerName();
        dto.Date = GetDateShift();
        dto.StartTime = GetStartTime();
        dto.EndTime = GetEndTime(dto.StartTime);
        dto.ShiftTime = GetShiftTime();
        

        WorkerShift result = await workerShiftService.CreateWorkerShift(dto);

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

    private async Task UpdateWorkerShift()
    {
        Console.Clear();

        WorkerShift selectedShift = await GetOptionWorkerShift("Select a shift to update");

        if (selectedShift == null)
            return;

        UpdateWorkerShiftDTO dto = new();

        bool updating = true;

        while (updating)
        {
            Console.Clear();

            AnsiConsole.Write(CreateUpdateView(selectedShift));

            UpdateOptions option = AnsiConsole.Prompt(
        new SelectionPrompt<UpdateOptions>()
            .Title("Select the field to update")
            .AddChoices(Enum.GetValues<UpdateOptions>()));

            switch (option)
            {
                case UpdateOptions.WorkerName:
                    dto.WorkerName = GetWorkerName();
                    break;
                case UpdateOptions.Date:
                    dto.Date = GetDateShift();
                    break;
                case UpdateOptions.StartTime:
                    dto.StartTime = GetStartTime();
                    break;
                case UpdateOptions.EndTime:
                    dto.EndTime = GetEndTime(dto.StartTime == null
                        ? selectedShift.StartTime
                        : (TimeOnly)dto.StartTime);
                    break;
                case UpdateOptions.ShiftTime:
                    dto.ShiftTime = GetShiftTime();
                    break;
                case UpdateOptions.SaveChanges:
                    updating = false;
                    break;
                case UpdateOptions.Cancel:
                    return;
            }

        }

        WorkerShift result = await workerShiftService.UpdateWorkerShift(selectedShift.ShiftId, dto);

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
    private async Task DeleteWorkerShift()
    {
        Console.Clear();

        WorkerShift selectedShift = await GetOptionWorkerShift("Select a shift to Delete");

        string result = await workerShiftService.DeleteWorkerShift(selectedShift.ShiftId);

        if (result == null)
        {
            AnsiConsole.MarkupLine("[#F0362C]Error deleting a worker shift[/]");
        }
        else
        {
            TableGenerator.CreateDetailsTable(selectedShift,"Shift Deleted");
            AnsiConsole.MarkupLine($"[#008148]{result}[/]");
        }

        AnsiConsole.WriteLine("Press any key to continue..");
        Console.ReadKey();
    }

    private string GetWorkerName()
    {
        string workerName = AnsiConsole.Ask<string>("Worker's name:");

        while (!ValidationInputUser.IsValidWorkerName(workerName))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid Name[/]");
            workerName = AnsiConsole.Ask<string>("Name: ");
        }

        return workerName;
    }

    private DateOnly GetDateShift()
    {
        string dateString = AnsiConsole.Ask<string>("Date's shift [#44BBA4](yyyy-MM-dd)[/]:");

        DateOnly date;

        while (!ValidationInputUser.IsValidDate(dateString, out date))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid Date[/]");
            dateString = AnsiConsole.Ask<string>("Date: ");
        }

        return date;
    }

    private TimeOnly GetStartTime()
    {
        string startTime = AnsiConsole.Ask<string>("StarTime [#44BBA4](HH:mm 24-hour format)[/]:");

        TimeOnly time;

        while (!ValidationInputUser.IsValidTime(startTime, out time))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid StarTime[/]");
            startTime = AnsiConsole.Ask<string>("Time: ");
        }

        return time;
    }

    private TimeOnly GetEndTime(TimeOnly startTime)
    {
        while (true)
        {
            string endTimeInput = AnsiConsole.Ask<string>("EndTime [#44BBA4](HH:mm 24-hour format)[/]:");

            if (!ValidationInputUser.IsValidTime(endTimeInput, out TimeOnly endTime))
            {
                AnsiConsole.MarkupLine("[#F0362C]Invalid EndTime[/]");
                continue;
            }

            if (ValidationInputUser.IsStartAndEndTimeEqual(startTime, endTime))
            {
                AnsiConsole.MarkupLine("[#F0362C]StartTime and EndTime cannot be the same[/]");
                continue;
            }

            return endTime;
        }
    }

    private static string GetShiftTime()
    {
        var options = AnsiConsole.Prompt(
            new SelectionPrompt<Enums.ShiftOptions>()
            .Title("Select an option")
            .AddChoices(Enum.GetValues<Enums.ShiftOptions>()));

        return options.ToString();
    }

    private async Task<WorkerShift> GetOptionWorkerShift(string message)
    {
        List<WorkerShift> workerShifts = await workerShiftService.GetAllWorkerShifts();

        if(workerShifts == null) 
        {
            return null;
        }

        WorkerShift selectedShift = AnsiConsole.Prompt(
            new SelectionPrompt<WorkerShift>()
            .Title($"{message}")
            .UseConverter(shift => $"{shift.WorkerName} | [#558B6E]({shift.Date:yyyy-MM-dd})[/] | {shift.ShiftTime}")
            .AddChoices(workerShifts));

        return selectedShift;
    }

    private Grid CreateUpdateView(WorkerShift shift)
    {
        var grid = new Grid();

        grid.AddColumn(new GridColumn().Width(70));
        grid.AddColumn(new GridColumn().Width(40));

        var detailsPanel = new Panel(
            TableGenerator.CreateDetailsTable(shift, null))
        {
            Header = new PanelHeader("Current Shift")
        };

        var instructionsPanel = new Panel(
            new Markup(
    @"[yellow]Update Worker Shift[/]

Select the field you want to update.

• Worker Name
• Date
• Start Time
• End Time
• Shift Time

[blue]Save Changes[/] to apply the modifications.
[red]Cancel[/] to discard them."))
        {
            Header = new PanelHeader("Instructions")
        };

        grid.AddRow(detailsPanel, instructionsPanel);

        return grid;
    }
}