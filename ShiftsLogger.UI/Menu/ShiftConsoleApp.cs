using ShiftsLogger.UI.Formatting;
using ShiftsLogger.UI.Models;
using ShiftsLogger.UI.Services;
using ShiftsLogger.UI.Validation;

namespace ShiftsLogger.UI.Menu;

/// <summary>
/// The whole user interface. It owns every prompt, every validation message and
/// every API call, so the API layer never has to guess what the user meant.
/// </summary>
public sealed class ShiftConsoleApp
{
    private readonly IShiftApiClient _client;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ShiftConsoleApp(IShiftApiClient client, TextReader input, TextWriter output)
    {
        _client = client;
        _input = input;
        _output = output;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        while (true)
        {
            PrintMenu();

            switch (ReadLine())
            {
                case "1":
                    await ListAllAsync(ct);
                    break;
                case "2":
                    await ViewByIdAsync(ct);
                    break;
                case "3":
                    await AddAsync(ct);
                    break;
                case "4":
                    await UpdateAsync(ct);
                    break;
                case "5":
                    await DeleteAsync(ct);
                    break;
                case "0":
                    _output.WriteLine("Goodbye.");
                    return;
                default:
                    _output.WriteLine("Please choose a number between 0 and 5.");
                    break;
            }
        }
    }

    private void PrintMenu()
    {
        _output.WriteLine();
        _output.WriteLine("ShiftsLogger");
        _output.WriteLine("1) List all shifts");
        _output.WriteLine("2) View a shift by id");
        _output.WriteLine("3) Add a shift");
        _output.WriteLine("4) Update a shift");
        _output.WriteLine("5) Delete a shift");
        _output.WriteLine("0) Exit");
        _output.Write("> ");
    }

    /// <summary>Returns "0" at end of input so a scripted reader exits cleanly (and tests terminate).</summary>
    private string ReadLine() => _input.ReadLine() ?? "0";

    private async Task ListAllAsync(CancellationToken ct)
    {
        try
        {
            var shifts = await _client.GetAllAsync(ct);
            foreach (var line in ShiftFormatter.FormatTable(shifts))
            {
                _output.WriteLine(line);
            }
        }
        catch (ShiftApiException ex)
        {
            _output.WriteLine(ex.Message);
        }
    }

    private async Task ViewByIdAsync(CancellationToken ct)
    {
        if (!TryReadInt("Shift id: ", out var id))
        {
            return;
        }

        try
        {
            var shift = await _client.GetByIdAsync(id, ct);
            if (shift is null)
            {
                _output.WriteLine($"No shift with id {id}.");
                return;
            }

            foreach (var line in ShiftFormatter.FormatTable(new[] { shift }))
            {
                _output.WriteLine(line);
            }
        }
        catch (ShiftApiException ex)
        {
            _output.WriteLine(ex.Message);
        }
    }

    private async Task AddAsync(CancellationToken ct)
    {
        var workerName = PromptWorkerName();
        if (workerName is null)
        {
            return;
        }

        if (!PromptDate("Start (yyyy-MM-dd HH:mm): ", out var start) ||
            !PromptDate("End   (yyyy-MM-dd HH:mm): ", out var end))
        {
            return;
        }

        var rangeError = ShiftInputValidator.ValidateTimes(start, end);
        if (rangeError is not null)
        {
            _output.WriteLine(rangeError);
            return;
        }

        try
        {
            var created = await _client.CreateAsync(
                new ShiftRequest { WorkerName = workerName, StartTime = start, EndTime = end }, ct);
            _output.WriteLine($"Saved shift #{created.Id}.");
        }
        catch (ShiftApiException ex)
        {
            _output.WriteLine($"Could not save the shift. {ex.Message}");
        }
    }

    private async Task UpdateAsync(CancellationToken ct)
    {
        if (!TryReadInt("Shift id to update: ", out var id))
        {
            return;
        }

        var workerName = PromptWorkerName();
        if (workerName is null)
        {
            return;
        }

        if (!PromptDate("New start (yyyy-MM-dd HH:mm): ", out var start) ||
            !PromptDate("New end   (yyyy-MM-dd HH:mm): ", out var end))
        {
            return;
        }

        var rangeError = ShiftInputValidator.ValidateTimes(start, end);
        if (rangeError is not null)
        {
            _output.WriteLine(rangeError);
            return;
        }

        try
        {
            var updated = await _client.UpdateAsync(id,
                new ShiftRequest { WorkerName = workerName, StartTime = start, EndTime = end }, ct);

            _output.WriteLine(updated is null ? $"No shift with id {id}." : $"Updated shift #{updated.Id}.");
        }
        catch (ShiftApiException ex)
        {
            _output.WriteLine($"Could not update the shift. {ex.Message}");
        }
    }

    private async Task DeleteAsync(CancellationToken ct)
    {
        if (!TryReadInt("Shift id to delete: ", out var id))
        {
            return;
        }

        try
        {
            _output.WriteLine(await _client.DeleteAsync(id, ct)
                ? $"Deleted shift #{id}."
                : $"No shift with id {id}.");
        }
        catch (ShiftApiException ex)
        {
            _output.WriteLine($"Could not delete the shift. {ex.Message}");
        }
    }

    /// <summary>Re-prompts until the name is valid; returns null at end of input.</summary>
    private string? PromptWorkerName()
    {
        while (true)
        {
            _output.Write("Worker name: ");
            var line = _input.ReadLine();
            if (line is null)
            {
                return null;
            }

            var error = ShiftInputValidator.ValidateWorkerName(line);
            if (error is null)
            {
                return line.Trim();
            }

            _output.WriteLine(error);
        }
    }

    /// <summary>Re-prompts until the date parses; returns false at end of input.</summary>
    private bool PromptDate(string label, out DateTime value)
    {
        value = default;

        while (true)
        {
            _output.Write(label);
            var line = _input.ReadLine();
            if (line is null)
            {
                return false;
            }

            if (ShiftInputValidator.TryParseDateTime(line, out value))
            {
                return true;
            }

            _output.WriteLine($"Please use the format {ShiftInputValidator.DateTimeFormat}.");
        }
    }

    private bool TryReadInt(string label, out int value)
    {
        while (true)
        {
            _output.Write(label);
            var line = _input.ReadLine();
            if (line is null)
            {
                value = 0;
                return false;
            }

            if (int.TryParse(line.Trim(), out value) && value > 0)
            {
                return true;
            }

            _output.WriteLine("Please enter a positive whole number.");
        }
    }
}
