using ShiftsLogger.UI.Formatting;
using ShiftsLogger.UI.Models;

namespace ShiftsLogger.Tests.Ui;

public class ShiftFormatterTests
{
    [Fact]
    public void FormatDuration_PrintsHoursAndMinutes()
    {
        var text = ShiftFormatter.FormatDuration(
            new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 16, 30, 0));

        Assert.Equal("8h 30m", text);
    }

    [Fact]
    public void FormatDuration_PadsMinutes()
    {
        var text = ShiftFormatter.FormatDuration(
            new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 8, 5, 0));

        Assert.Equal("0h 05m", text);
    }

    [Fact]
    public void FormatDuration_HandlesShiftsLongerThanADay()
    {
        var text = ShiftFormatter.FormatDuration(
            new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 15, 10, 0, 0));

        Assert.Equal("26h 00m", text);
    }

    [Fact]
    public void FormatDuration_ReturnsInvalid_WhenEndIsBeforeStart()
    {
        var text = ShiftFormatter.FormatDuration(
            new DateTime(2026, 9, 14, 16, 0, 0), new DateTime(2026, 9, 14, 8, 0, 0));

        Assert.Equal("invalid", text);
    }

    [Fact]
    public void FormatTable_ReturnsMessage_WhenThereAreNoShifts()
    {
        var lines = ShiftFormatter.FormatTable(Array.Empty<ShiftDto>());

        Assert.Equal(new[] { "No shifts recorded yet." }, lines);
    }

    [Fact]
    public void FormatTable_AlignsColumnsAndIncludesDuration()
    {
        var shifts = new[]
        {
            new ShiftDto(1, "Ana", new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 16, 30, 0)),
            new ShiftDto(2, "Bea", new DateTime(2026, 9, 15, 9, 0, 0), new DateTime(2026, 9, 15, 17, 0, 0))
        };

        var lines = ShiftFormatter.FormatTable(shifts);

        Assert.Equal("Id | Worker | Start            | End              | Duration", lines[0]);
        Assert.Equal("1  | Ana    | 2026-09-14 08:00 | 2026-09-14 16:30 | 8h 30m", lines[1]);
        Assert.Equal("2  | Bea    | 2026-09-15 09:00 | 2026-09-15 17:00 | 8h 00m", lines[2]);
    }
}
