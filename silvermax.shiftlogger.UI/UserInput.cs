using silvermax.shiftlogger.UI.Models;
using Spectre.Console;
using System.Globalization;
using static silvermax.shiftlogger.UI.Enums;

namespace silvermax.shiftlogger.UI;

internal static class UserInput
{
    public static ShiftName GetShiftName() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<ShiftName>()
            .Title("Which shift is this?")
            .AddChoices(Enum.GetValues<ShiftName>()));

    public static DateTime GetShiftDate() =>
        AnsiConsole.Ask<DateTime>("Shift Date (yyyy-MM-dd):");

    public static CreateNewShiftDto GetNewShift()
    {
        var name = GetShiftName();
        var date = GetShiftDate();

        var (startHour, endHour) = name switch
        {
            ShiftName.Morning => (6, 12),
            ShiftName.Afternoon => (12, 17),
            ShiftName.Evening => (17, 21),
            ShiftName.Night => (21, 6),
            _ => throw new ArgumentOutOfRangeException(nameof(name))
        };

        var defaultStart = date.Date.AddHours(startHour);
        var defaultEnd = endHour <= startHour
            ? date.Date.AddDays(1).AddHours(endHour)
            : date.Date.AddHours(endHour);

        AnsiConsole.MarkupLine($"Default: [purple]{defaultStart:yyyy-MM-dd HH:mm}[/] → [purple]{defaultEnd:yyyy-MM-dd HH:mm}[/]  (Duration: {defaultEnd - defaultStart})");

        if (AnsiConsole.Confirm("Use the default start/end times?"))
        {
            return new CreateNewShiftDto(name.ToString(), defaultStart, defaultEnd);
        }

        var startTimeOnly = AskTimeOnly("Enter start time (HH:mm):");
        DateTime startDateTime = date.Date.Add(startTimeOnly.ToTimeSpan());

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Specify end by:")
            .AddChoices(new[] { "End time", "Duration (hours)" }));

        DateTime endDateTime;
        if (choice == "End time")
        {
            while (true)
            {
                var endTimeOnly = AskTimeOnly("Enter end time (HH:mm):");
                var tentativeEnd = date.Date.Add(endTimeOnly.ToTimeSpan());

                if (tentativeEnd > startDateTime)
                {
                    endDateTime = tentativeEnd;
                    break;
                }

                if (AnsiConsole.Confirm("End time is not after start. Treat end time as next day (overnight)?"))
                {
                    endDateTime = tentativeEnd.AddDays(1);
                    break;
                }

                AnsiConsole.MarkupLine("[yellow]Please re-enter a valid end time.[/]");
            }
        }
        else
        {
            int hours;
            while (true)
            {
                hours = AnsiConsole.Ask<int>("Enter shift duration in hours (e.g., 8 or 12):");
                if (hours > 0) break;
                AnsiConsole.MarkupLine("[red]Duration must be a positive integer.[/]");
            }

            endDateTime = startDateTime.AddHours(hours);
        }

        return new CreateNewShiftDto(name.ToString(), startDateTime, endDateTime);
    }

    private static TimeOnly AskTimeOnly(string prompt)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (TimeOnly.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out var t))
                return t;

            if (TimeOnly.TryParseExact(input, new[] { "H:mm", "HH:mm", "h:mm tt", "hh:mm tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out t))
                return t;

            AnsiConsole.MarkupLine("[red]Invalid time format. Use HH:mm (e.g. 08:00 or 21:30).[/]");
        }
    }
}
