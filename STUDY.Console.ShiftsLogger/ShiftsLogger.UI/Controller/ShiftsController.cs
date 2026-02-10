using ShiftsLogger.UI.DTOs;
using ShiftsLogger.UI.Services;
using Spectre.Console;

namespace ShiftsLogger.UI.Controller;

public class ShiftsController
{
    private readonly ShiftService _shiftService = new();
    public async Task AddShift(WorkerDto workerDto)
    {
        bool isValid = false;
        DateTime startTime = default;
        DateTime endTime = default;
        while (!isValid)
        {
            //TODO: Show valid Formats for user
            startTime = AnsiConsole.Ask<DateTime>("Enter start time: ");
            endTime = AnsiConsole.Ask<DateTime>("Enter end time: ");
            if (endTime <= startTime)
            {
                AnsiConsole.MarkupLine("[Red]End time is not valid. Please try again:[/]");
            }
            else
            {
                isValid = true;
            }
        }

        ShiftCreationDto shiftCreationDto = new()
        {
            StartTime = startTime,
            EndTime = endTime,
            WorkerId = workerDto.Id
        };
        var shiftIsCreated = await _shiftService.AddShift(shiftCreationDto);
        if (shiftIsCreated)
        {
            AnsiConsole.MarkupLine("[green]Shift created successfully![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Shift not created![/]");
        }
    }
}