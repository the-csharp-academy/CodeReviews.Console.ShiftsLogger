using silvermax.shiftlogger.UI.Models;
using Spectre.Console;
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

        var startTime = date.Date.AddHours(startHour);
        var endTime = endHour <= startHour
            ? date.Date.AddDays(1).AddHours(endHour)
            : date.Date.AddHours(endHour);

        return new CreateNewShiftDto(name.ToString(), startTime, endTime);
    }
}
