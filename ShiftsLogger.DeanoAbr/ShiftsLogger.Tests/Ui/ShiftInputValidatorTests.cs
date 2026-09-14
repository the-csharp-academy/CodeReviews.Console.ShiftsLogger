using ShiftsLogger.UI.Validation;

namespace ShiftsLogger.Tests.Ui;

public class ShiftInputValidatorTests
{
    [Theory]
    [InlineData("2026-09-14 08:00", true)]
    [InlineData("2026-09-14T08:00", false)]
    [InlineData("14/09/2026 08:00", false)]
    [InlineData("not a date", false)]
    [InlineData("", false)]
    public void TryParseDateTime_AcceptsOnlyTheDocumentedFormat(string input, bool expected)
    {
        Assert.Equal(expected, ShiftInputValidator.TryParseDateTime(input, out _));
    }

    [Fact]
    public void TryParseDateTime_ParsesTheValue()
    {
        var ok = ShiftInputValidator.TryParseDateTime("2026-09-14 08:00", out var value);

        Assert.True(ok);
        Assert.Equal(new DateTime(2026, 9, 14, 8, 0, 0), value);
    }

    [Fact]
    public void ValidateTimes_ReturnsNull_WhenEndIsAfterStart()
    {
        var error = ShiftInputValidator.ValidateTimes(
            new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 16, 30, 0));

        Assert.Null(error);
    }

    [Fact]
    public void ValidateTimes_ReturnsError_WhenEndIsBeforeStart()
    {
        var error = ShiftInputValidator.ValidateTimes(
            new DateTime(2026, 9, 14, 16, 0, 0), new DateTime(2026, 9, 14, 8, 0, 0));

        Assert.Equal("End time must be after the start time.", error);
    }

    [Fact]
    public void ValidateTimes_ReturnsError_WhenEndEqualsStart()
    {
        var same = new DateTime(2026, 9, 14, 8, 0, 0);

        Assert.NotNull(ShiftInputValidator.ValidateTimes(same, same));
    }

    [Theory]
    [InlineData("Ana", null)]
    [InlineData("  Ana  ", null)]
    [InlineData("", "Worker name is required.")]
    [InlineData("   ", "Worker name is required.")]
    public void ValidateWorkerName_ChecksPresence(string input, string? expected)
    {
        Assert.Equal(expected, ShiftInputValidator.ValidateWorkerName(input));
    }

    [Fact]
    public void ValidateWorkerName_RejectsNamesLongerThan100Characters()
    {
        Assert.NotNull(ShiftInputValidator.ValidateWorkerName(new string('a', 101)));
    }
}
