using System.Text.RegularExpressions;

namespace ShiftLogger.UI.Extensions;

internal static class EnumExtensions
{
    internal static string ToDisplayString(this Enum value)
    {
        return Regex.Replace(value.ToString(), "(?<!^)([A-Z])", " $1");
    }
}