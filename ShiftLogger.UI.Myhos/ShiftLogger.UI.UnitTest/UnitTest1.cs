namespace ShiftLogger.UI.UnitTest;

public class Tests
{
    private static readonly TestCaseData[] WorkerNameTestCases =
        {
            new("Juan Pérez", true),
            new("María Gómez", true),
            new("Carlos Rodríguez", true),
            new("Ana", true),
            new("José Luis", true),
            new("María Fernanda", true),
            new("Ángel", true),
            new("Óscar", true),

            new("", false),
            new(" ", false),
            new((string?)null, false),

            new("12345", false),
            new("Juan1", false),
            new("Carlos2026", false),
            new("Pedro5Ramirez", false),
            new("@Juan", false),
            new("Juan!", false),
        };

    private static readonly TestCaseData[] DateTestCases =
        {
            new("2026-07-20", true),
            new("2026-12-31", true),
            new("2028-02-29", true),
            new("01/01/2026", true),

            new("", false),
            new(" ", false),
            new("abc", false),
            new("31/02/2026", false),
            new("29/02/2027", false),
            new("2026-13-01", false),
            new("2026-00-15", false),
            new("99/99/9999", false),
        };
    private static readonly TestCaseData[] TimeTestCases =
        {
            new("00:00", true),
            new("06:00", true),
            new("08:30", true),
            new("12:00", true),
            new("14:45", true),
            new("18:15", true),
            new("23:59", true),

            new("", false),
            new(" ", false),
            new("abc", false),
            new("24:00", false),
            new("25:30", false),
            new("12:60", false),
            new("99:99", false),
            new("-01:00", false),
            new("14-30", false),
            new("14.30", false),
        };

    private static readonly TestCaseData[] EqualTimeTestCases =
        {
            new(new TimeOnly(8, 0), new TimeOnly(8, 0), true),
            new(new TimeOnly(0, 0), new TimeOnly(0, 0), true),
            new(new TimeOnly(23, 59), new TimeOnly(23, 59), true),

            new(new TimeOnly(8, 0), new TimeOnly(16, 0), false),
            new(new TimeOnly(6, 30), new TimeOnly(6, 31), false),
            new(new TimeOnly(23, 59), new TimeOnly(0, 0), false),
            new(new TimeOnly(12, 15), new TimeOnly(12, 45), false),
        };

    [TestCaseSource(nameof(WorkerNameTestCases))]
    public void IsValidWorkerName_ReturnsExpectedResult(string? workerName, bool expectedResult)
    {
        bool result = ValidationInputUser.IsValidWorkerName(workerName);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCaseSource(nameof(DateTestCases))]
    public void IsValidDate_ReturnsExpectedResult(string input, bool expectedResult)
    {
        bool result = ValidationInputUser.IsValidDate(input, out _);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCaseSource(nameof(TimeTestCases))]
    public void IsValidTime_ReturnsExpectedResult(string input, bool expectedResult)
    {
        bool result = ValidationInputUser.IsValidTime(input, out _);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCaseSource(nameof(EqualTimeTestCases))]
    public void IsStartAndEndTimeEqual_ReturnsExpectedResult(TimeOnly startTime,TimeOnly endTime,bool expectedResult)
    {
        bool result = ValidationInputUser.IsStartAndEndTimeEqual(startTime, endTime);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}