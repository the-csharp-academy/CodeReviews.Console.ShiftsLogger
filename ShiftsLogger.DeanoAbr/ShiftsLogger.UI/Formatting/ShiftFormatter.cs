using System.Globalization;
using ShiftsLogger.UI.Models;

namespace ShiftsLogger.UI.Formatting;

public static class ShiftFormatter
{
    private const string DatePattern = "yyyy-MM-dd HH:mm";
    private const int IdWidth = 2;
    private const int WorkerWidth = 6;
    private const int DateWidth = 16;
    private const int DurationWidth = 8;

    /// <summary>Renders a shift length as "8h 30m".</summary>
    public static string FormatDuration(DateTime start, DateTime end)
    {
        var duration = end - start;

        if (duration < TimeSpan.Zero)
        {
            return "invalid";
        }

        var hours = (long)duration.TotalHours;
        return $"{hours}h {duration.Minutes:D2}m";
    }

    /// <summary>Renders shifts as a fixed-width text table.</summary>
    public static IReadOnlyList<string> FormatTable(IEnumerable<ShiftDto> shifts)
    {
        var rows = shifts.ToList();

        if (rows.Count == 0)
        {
            return new[] { "No shifts recorded yet." };
        }

        var workerWidth = Math.Max(WorkerWidth, rows.Max(s => s.WorkerName.Length));

        var lines = new List<string>
        {
            $"{Pad("Id", IdWidth)} | {Pad("Worker", workerWidth)} | {Pad("Start", DateWidth)} | {Pad("End", DateWidth)} | Duration"
        };

        lines.AddRange(rows.Select(s =>
            $"{Pad(s.Id.ToString(CultureInfo.InvariantCulture), IdWidth)} | " +
            $"{Pad(s.WorkerName, workerWidth)} | " +
            $"{Pad(s.StartTime.ToString(DatePattern, CultureInfo.InvariantCulture), DateWidth)} | " +
            $"{Pad(s.EndTime.ToString(DatePattern, CultureInfo.InvariantCulture), DateWidth)} | " +
            $"{FormatDuration(s.StartTime, s.EndTime)}"));

        return lines;
    }

    private static string Pad(string value, int width) =>
        value.Length >= width ? value : value.PadRight(width);
}
