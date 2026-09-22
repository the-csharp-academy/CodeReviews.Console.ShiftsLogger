using ShiftLogger.Application.Shifts.Dtos;
using ShiftLogger.Console.Presentation.Output;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Views;

internal class ShiftTableView
{
    public void Render(List<ShiftDto>? shifts)
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Clock-In Date");
        table.AddColumn("Clock-In Time");
        table.AddColumn("Clock-Out Date");
        table.AddColumn("Clock-Out Time");
        table.AddColumn("Duration");

        if (shifts.Count == 0)
            table.AddRow("1", "<EMPTY>", "<EMPTY>", "<EMPTY>", "<EMPTY>", "N/A");
        else
        {
            int id = 1;
            foreach (var shift in shifts)
            {
                table.AddRow(
                    $"{id}",
                    shift.ClockInTime.ToString("yyyy-MM-dd"),
                    shift.ClockInTime.ToString("HH:mm:ss"),
                    shift.ClockOutTime.ToString("yyyy-MM-dd"),
                    shift.ClockOutTime.ToString("HH:mm:ss"),
                    $"{Messages.GenerateDurationString(shift.ClockInTime, shift.ClockOutTime)}");

                id++;
            }
        }

        AnsiConsole.Write(table);
    }
}