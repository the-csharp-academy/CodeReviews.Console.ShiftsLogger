using ShiftLoggerConsole.Models;
using ShiftLoggerConsole.Api;
using ShiftLoggerConsole.Views.Utilities;
using Spectre.Console;

namespace ShiftLoggerConsole.Handlers;
internal class ShiftHandler
{
    private ManagementApiClient _client;

    public ShiftHandler()
    {
        _client = new ManagementApiClient();
    }


    public async Task LogShift()
    {
        Helpers.Header($"Logging New Shift");

        var (shiftStarted, shiftEnded) = Helpers.GetValidatedShiftSpan();

        TimeSpan shiftDuration = shiftEnded - shiftStarted;

        List<EmployeeResponse> employees;

        try
        {
            employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
            ?? new List<EmployeeResponse>();
        } 
        catch(Exception ex)
        {
            employees = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] No employees to assign shift to. Create a new employee before creating a shift.\n");
            Helpers.Pause();
            return;
        }

        Helpers.Header($"Logging Shift: {shiftDuration}");

        var employee = AnsiConsole.Prompt(
            new SelectionPrompt<EmployeeResponse>()
            .Title("Please select an [chartreuse1]employee[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(employees));

        var createReq = new ShiftRequest
        {
            Start = shiftStarted,
            End = shiftEnded,
            EmployeeId = employee.Id
        };

        try
        {
            await _client.PostAsync<ShiftRequest, ShiftResponse>("shifts", createReq);
            AnsiConsole.MarkupLine($"\n[chartreuse1]Success:[/] Shift ({shiftDuration}) has been successfully logged for {employee.Name}\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        Helpers.Pause();
    }

    public async Task GetAllShifts()
    {
        Helpers.Header($"Viewing all Shifts (Ordered Descending)");

        List<ShiftResponse> shifts;

        try
        {
            shifts = await _client.GetAsync<List<ShiftResponse>>(endpoint: "shifts")
            ?? new List<ShiftResponse>();
            shifts.Reverse();
        }
        catch (Exception ex)
        {
            shifts = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] No shifts to display.\n");
            Helpers.Pause();
            return;
        }

        var table = new Table();
        table
            .AddColumn("[black on chartreuse1] Shift Start [/]")
            .AddColumn("[black on chartreuse1] Shift End [/]")
            .AddColumn("[black on chartreuse1] Shift Duration [/]")
            .AddColumn("[black on chartreuse1] Employee ID [/]")
            .ShowRowSeparators()
            .Border(TableBorder.Minimal)
            .BorderStyle(new Style(Color.Grey23))
            .Expand();

        foreach (var s in shifts)
        {
            table.AddRow(
                $"{s.Start}",
                $"{s.End}",
                $"{s.Duration}",
                $"{s.EmployeeId}");
        }

        AnsiConsole.Write(table);

        Helpers.Pause();
    }

    public async Task GetEmployeeShifts()
    {
        List<EmployeeResponse> employees;
        List<ShiftResponse> shifts;

        Helpers.Header("View Shifts by Employee");

        try
        {
            employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
            ?? new List<EmployeeResponse>();
        }
        catch (Exception ex)
        {
            employees = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] There are no employees currently in the database.\n");
            Helpers.Pause();
            return;
        }

        var employee = AnsiConsole.Prompt(
            new SelectionPrompt<EmployeeResponse>()
            .Title("Please select an [chartreuse1]employee[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(employees));

        try
        {
            shifts = await _client.GetAsync<List<ShiftResponse>>(endpoint: $"employees/{employee.Id}/shifts")
            ?? new List<ShiftResponse>();
            shifts.Reverse();

            if (shifts.Count == 0)
            {
                AnsiConsole.MarkupLine($"\n{employee.Name} has no logged shifts to display.\n");
                Helpers.Pause();
                return;
            }
        }
        catch (Exception ex)
        {
            shifts = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        Helpers.Header($"(Employee ID: {employee.Id}) | {employee.Name}'s Shifts (Ordered Descending)");

        var table = new Table();
        table
            .AddColumn("[black on chartreuse1] Shift Start [/]")
            .AddColumn("[black on chartreuse1] Shift End [/]")
            .AddColumn("[black on chartreuse1] Shift Duration [/]")
            .ShowRowSeparators()
            .Border(TableBorder.Minimal)
            .BorderStyle(new Style(Color.Grey23))
            .Expand();

        foreach (var s in shifts)
        {
            table.AddRow(
                $"{s.Start}",
                $"{s.End}",
                $"{s.Duration}");
        }

        AnsiConsole.Write(table);

        Helpers.Pause();
    }

    public async Task UpdateShift()
    {
        Helpers.Header($"Select a Shift to Update");

        List<EmployeeResponse> employees;
        List<ShiftResponse> shifts;

        try
        {
            shifts = await _client.GetAsync<List<ShiftResponse>>(endpoint: "shifts")
            ?? new List<ShiftResponse>();
            shifts.Reverse();
        }
        catch (Exception ex)
        {
            shifts = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] No shifts to display.\n");
            Helpers.Pause();
            return;
        }

        var shift = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftResponse>()
            .Title("Please select a [chartreuse1]shift[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(shifts));

        var updateTimes =
            Helpers.ConfirmationPrompt("Update Shift Start & End Time?", defaultValue: false);

        var updateEmployee = 
            Helpers.ConfirmationPrompt("Update Shift Employee Record?", defaultValue: false);

        Helpers.Header($"Updating Shift Times");

        ShiftRequest updatedReq = new ShiftRequest();

        if (updateTimes)
        {
            var (shiftStarted, shiftEnded) = Helpers.GetValidatedShiftSpan();
            updatedReq.Start = shiftStarted;
            updatedReq.End = shiftEnded;
        }
        else
        {
            updatedReq.Start = shift.Start;
            updatedReq.End = shift.End;
        }

        Helpers.Header($"Updating Shift Employee Record");

        if (updateEmployee)
        {
            try
            {
                employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
                ?? new List<EmployeeResponse>();
            }
            catch (Exception ex)
            {
                employees = new();
                AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
                Helpers.Pause();
                return;
            }

            if (employees.Count == 0)
            {
                AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] There are no employees currently in the database.\n");
                Helpers.Pause();
                return;
            }

            var employee = AnsiConsole.Prompt(
            new SelectionPrompt<EmployeeResponse>()
            .Title("Please select an [chartreuse1]employee[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(employees));

            updatedReq.EmployeeId = employee.Id;
        }
        else
        {
            updatedReq.EmployeeId = shift.EmployeeId;
        }

        try
        {
            await _client.PutAsync<ShiftRequest>($"shifts/{shift.Id}", updatedReq);
            AnsiConsole.MarkupLine($"[chartreuse1]Success:[/] Shift with ID {shift.Id} has been updated.");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        Helpers.Pause();
    }

    public async Task DeleteShift()
    {
        Helpers.Header($"Select a Shift to Delete");

        List<ShiftResponse> shifts;

        try
        {
            shifts = await _client.GetAsync<List<ShiftResponse>>(endpoint: "shifts")
            ?? new List<ShiftResponse>();
            shifts.Reverse();
        }
        catch (Exception ex)
        {
            shifts = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] No shifts to display.\n");
            Helpers.Pause();
            return;
        }

        var shift = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftResponse>()
            .Title("Please select a [chartreuse1]shift[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(shifts));

        try
        {
            await _client.DeleteAsync($"shifts/{shift.Id}");
            AnsiConsole.MarkupLine($"\n[chartreuse1]Success:[/] Shift ({shift.ToString()}) has been successfully deleted.");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        Helpers.Pause();
    }  
}
