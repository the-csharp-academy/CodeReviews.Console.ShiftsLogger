using System.Globalization;

namespace ShiftsLogger.UI.Validation;

/// <summary>All user-input rules live here so they can be unit tested without a console.</summary>
public static class ShiftInputValidator
{
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    public const int MaxWorkerNameLength = 100;

    public static bool TryParseDateTime(string? input, out DateTime value) =>
        DateTime.TryParseExact(
            input?.Trim(),
            DateTimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out value);

    public static string? ValidateTimes(DateTime start, DateTime end) =>
        end <= start ? "End time must be after the start time." : null;

    public static string? ValidateWorkerName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Worker name is required.";
        }

        return name.Trim().Length > MaxWorkerNameLength
            ? $"Worker name must be {MaxWorkerNameLength} characters or fewer."
            : null;
    }
}
