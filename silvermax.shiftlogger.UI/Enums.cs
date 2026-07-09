using System.Text.RegularExpressions;

namespace silvermax.shiftlogger.UI;

internal class Enums
{
    internal static string DisplayChoice(Enum value) =>
        Regex.Replace(value.ToString(), "(?<!^)([A-Z])", " $1");

    internal enum MenuOptions
    {
        AddShift,
        UpdateShift,
        SeeAllShifts,
        DeleteShift,
        Exit
    }

    internal enum ShiftName
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }
}
