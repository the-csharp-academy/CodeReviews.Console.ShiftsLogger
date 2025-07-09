using Shifts_Logger_UI.Controllers;
using Shifts_Logger_UI.Models;
using Spectre.Console;
using Table = Spectre.Console.Table;

namespace Shifts_Logger_UI.Views;

public class UserInterface
{
    private readonly ShiftsController _shiftsController = new();

    internal async Task ViewAllShifts()
    {
        var shifts = await _shiftsController.GetAllShiftsAsync(_shiftsController.Client);

        if (!shifts.Any())
            AnsiConsole.MarkupLine("[red]No shifts found.[/]");

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Employee Name");
        table.AddColumn("Position");
        table.AddColumn("Shift Number");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (var shift in shifts)
            table.AddRow(
                shift.Id.ToString(),
                shift.EmployeeName,
                shift.Position,
                shift.ShiftNumber.ToString(),
                shift.StartTime.ToString("g"),
                shift.EndTime.ToString("g"),
                $"{Math.Floor(shift.Duration.TotalHours)} hours {shift.Duration.TotalMinutes % 60} minutes"
            );

        table.Expand();
        AnsiConsole.Write(table);
    }

    internal async Task ViewShiftsById(Shift shift)
    {
        if (shift == null)
        {
            AnsiConsole.MarkupLine("[red]Shift not found.[/]");
            return;
        }
        var panel = new Panel(
            $"Id: {shift.Id} \nName: {shift.EmployeeName} \nPosition: {shift.Position} \nShift Number: {shift.ShiftNumber} \nStart Time: {shift.StartTime} \nEnd Time: {shift.EndTime} \nDuration: {Math.Floor(shift.Duration.TotalHours)} hours {shift.Duration.TotalMinutes % 60} minutes"
        );
        panel.Header($"Shift Details for ID: {shift.Id}");
        panel.Padding = new Padding(2);

        AnsiConsole.Write(panel);
    }
}
