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

    public async Task UpdateShift(WorkerDto workerDto)
    {
        var workerShifts = await _shiftService.GetShiftsByWorkerId(workerDto.Id);
        if (workerShifts is null)
        {
            return;
        }

        var shiftChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftDto>()
                .Title("Choose a Shift to Update:")
                .AddChoices(workerShifts)
        );
        var updateOption = AnsiConsole.Prompt<string>(
            new SelectionPrompt<string>()
                .Title($"Update for shift: {shiftChoice}")
                .AddChoices("Update Start Time", "Update End Time", "Update Start Time and End Time", "Cancel")
        );
        DateTime startTime;
        DateTime endTime;
        switch (updateOption)
        {
            case "Update Start Time":
                startTime = AnsiConsole.Ask<DateTime>("Enter new start time: ");
                while (startTime >= shiftChoice.EndTime)
                {
                    AnsiConsole.MarkupLine("[Red]Start time is not valid. Please try again:[/]");
                    startTime = AnsiConsole.Ask<DateTime>("Enter new start time: ");
                }
                shiftChoice.StartTime = startTime;
                break;
            case "Update End Time":
                endTime = AnsiConsole.Ask<DateTime>("Enter new end time: ");
                while (endTime <= shiftChoice.StartTime)
                {
                    AnsiConsole.MarkupLine("[Red]End time is not valid. Please try again:[/]");
                    endTime = AnsiConsole.Ask<DateTime>("Enter new end time: ");
                }

                shiftChoice.EndTime = endTime;
                break;
            case "Update Start Time and End Time":
                 startTime = AnsiConsole.Ask<DateTime>("Enter new start time: ");
                 endTime = AnsiConsole.Ask<DateTime>("Enter new end time: ");
                 while (endTime <= startTime)
                 {
                     AnsiConsole.MarkupLine("[Red]End time is not valid. Please try again:[/]");
                     endTime = AnsiConsole.Ask<DateTime>("Enter new end time: ");
                 }
                 shiftChoice.StartTime = startTime;
                 shiftChoice.EndTime = endTime;
                break;
            case "Cancel":
                return;
        }

        var shiftIsUpdated = await _shiftService.UpdateShift(shiftChoice);
        if(shiftIsUpdated)
            AnsiConsole.MarkupLine("[green]Shift Updated Successfully![/]");
        else
            AnsiConsole.MarkupLine("[red]Shift is NOT Updated[/]");
    }

    public async Task DeleteShift(WorkerDto workerDto)
    {
        var workerShifts = await _shiftService.GetShiftsByWorkerId(workerDto.Id);
        if (workerShifts is null)
        {
            return;
        }

        var shiftChoice = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftDto>()
                .Title("Choose a Shift to Delete:")
                .AddChoices(workerShifts)
        );
        
        var shiftIsDeleted = await _shiftService.DeleteShift(shiftChoice.Id);
        if (shiftIsDeleted)
            AnsiConsole.MarkupLine("[green]Shift Deleted Successfully![/]");
        else
            AnsiConsole.MarkupLine("[red]Shift is NOT Deleted[/]");
    }
}