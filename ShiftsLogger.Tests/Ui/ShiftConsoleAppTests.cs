using ShiftsLogger.UI.Models;
using ShiftsLogger.UI.Menu;

namespace ShiftsLogger.Tests.Ui;

public class ShiftConsoleAppTests
{
    private static async Task<string> RunAsync(string scriptedInput, FakeShiftApiClient client)
    {
        var output = new StringWriter();
        var app = new ShiftConsoleApp(client, new StringReader(scriptedInput), output);
        await app.RunAsync();
        return output.ToString();
    }

    [Fact]
    public async Task Exit_Option_StopsTheLoop()
    {
        var client = new FakeShiftApiClient();

        var output = await RunAsync("0\n", client);

        Assert.Contains("Goodbye.", output);
    }

    [Fact]
    public async Task Unknown_Option_PrintsGuidanceThenContinues()
    {
        var client = new FakeShiftApiClient();

        var output = await RunAsync("9\n0\n", client);

        Assert.Contains("Please choose a number between 0 and 5.", output);
        Assert.Contains("Goodbye.", output);
    }

    [Fact]
    public async Task AddShift_RejectsBadDateThenSavesValidShift()
    {
        var client = new FakeShiftApiClient();
        var input = string.Join("\n", "3", "Ana", "14/09/2026 08:00", "2026-09-14 08:00", "2026-09-14 16:30", "0") + "\n";

        var output = await RunAsync(input, client);

        Assert.Contains("Please use the format yyyy-MM-dd HH:mm.", output);
        Assert.Contains("Saved shift #1", output);
        var request = Assert.Single(client.Created);
        Assert.Equal("Ana", request.WorkerName);
        Assert.Equal(new DateTime(2026, 9, 14, 16, 30, 0), request.EndTime);
    }

    [Fact]
    public async Task AddShift_RejectsEndBeforeStart_WithoutCallingTheApi()
    {
        var client = new FakeShiftApiClient();
        var input = string.Join("\n", "3", "Ana", "2026-09-14 16:00", "2026-09-14 08:00", "0") + "\n";

        var output = await RunAsync(input, client);

        Assert.Contains("End time must be after the start time.", output);
        Assert.Empty(client.Created);
    }

    [Fact]
    public async Task AddShift_ShowsFriendlyMessage_WhenApiIsDown()
    {
        var client = new FakeShiftApiClient { ThrowUnreachable = true };
        var input = string.Join("\n", "3", "Ana", "2026-09-14 08:00", "2026-09-14 16:30", "0") + "\n";

        var output = await RunAsync(input, client);

        Assert.Contains("Could not reach the Shifts API", output);
        Assert.Contains("Goodbye.", output);
    }

    [Fact]
    public async Task ListShifts_PrintsNoShiftsMessage_WhenEmpty()
    {
        var client = new FakeShiftApiClient();

        var output = await RunAsync("1\n0\n", client);

        Assert.Contains("No shifts recorded yet.", output);
    }

    [Fact]
    public async Task ListShifts_PrintsTableRow_WithDuration()
    {
        var client = new FakeShiftApiClient();
        client.Shifts.Add(new ShiftDto(1, "Ana",
            new DateTime(2026, 9, 14, 8, 0, 0), new DateTime(2026, 9, 14, 16, 30, 0)));

        var output = await RunAsync("1\n0\n", client);

        Assert.Contains("8h 30m", output);
        Assert.Contains("Ana", output);
    }
}
