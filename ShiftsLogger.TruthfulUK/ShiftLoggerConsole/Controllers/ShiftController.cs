using ShiftLoggerConsole.Models;
using ShiftLoggerConsole.Services;
using ShiftLoggerConsole.Views.Helpers;
using Spectre.Console;

namespace ShiftLoggerConsole.Controllers;
internal class ShiftController
{
    private ManagementService _client;

    public ShiftController()
    {
        _client = new ManagementService();
    }


    public async Task LogShift()
    {
        InputHelper.Header("Log a Shift");

        var (shiftStarted, shiftEnded) = InputHelper.GetValidatedShiftSpan();

        TimeSpan shiftDuration = shiftEnded - shiftStarted;

        InputHelper.Header($"Log a Shift: {shiftDuration}");

        string endpoint = "employees";

        var employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint)
            ?? new List<EmployeeResponse>();

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
            var updated = _client.PostAsync<ShiftRequest, ShiftResponse>("shifts", createReq);
            await updated;
            AnsiConsole.MarkupLine($"\n Shift ({shiftDuration}) has been successfully logged for {employee.Name}\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        InputHelper.Pause();
    }

    public async Task<List<ShiftResponse>> GetAllShifts()
    {
        string endpoint = "shifts";
        var res = await _client.GetAsync<List<ShiftResponse>>(endpoint);

        return res ?? new List<ShiftResponse>();
    }
}
