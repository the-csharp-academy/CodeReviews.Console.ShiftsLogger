using System.Text.RegularExpressions;

namespace ShiftLogger.UI;

public class ValidationInputUser
{
    public static bool IsValidWorkerName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return Regex.IsMatch(name, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$");
    }

    public static bool IsValidDate(string input, out DateOnly date)
    {
        return DateOnly.TryParse(input, out date);
    }

    public static bool IsStartAndEndTimeEqual(TimeOnly startTime, TimeOnly endTime)
    {
        return startTime == endTime;
    }

    public static bool IsValidTime(string input, out TimeOnly time)
    {
        return TimeOnly.TryParse(input, out time);
    }
}