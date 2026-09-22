using ShiftLogger.Application.Shifts.Commands.CreateShift;
using ShiftLogger.Console.ApiClients.Shifts;
using ShiftLogger.Console.Presentation.Models;
using ShiftLogger.Console.Presentation.Output;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Services;

internal class CreateShiftService
{
    private readonly IShiftApiClient _shiftApiClient;

    public CreateShiftService(IShiftApiClient shiftApiClient)
    {
        _shiftApiClient = shiftApiClient;
    }

    public async Task RunAsync(EmployeeViewModel employee)
    {
        var enterShift = true;

        while (enterShift)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"Enter new shift for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]");
            Messages.PrintBlankLines(2);

            var startTime = UserInput.GetTimeFromUser("Enter shift start time (Format: [yellow]yyyy-MM-dd HH:mm:ss[/]): ");
            var endTime = UserInput.GetTimeFromUser("Enter shift end time (Format: [yellow]yyyy-MM-dd HH:mm:ss[/]): ");

            Messages.PrintBlankLines(1);
            var confirmAdd = UserInput.GetConfirmation($"Add the following shift to work log for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]:" +
                $"\r\n\tClock-in Time:\t[green]{startTime}[/]" +
                $"\r\n\tClock-Out Time:\t[green]{endTime}[/]" +
                $"\r\n\tDuration:\t[green]{Messages.GenerateDurationString(startTime, endTime)}[/]" +
                $"\r\n\r\nConfirm add::");

            Messages.PrintBlankLines(1);
            if (confirmAdd)
            {
                var command = new CreateShiftCommand(employee.Id, startTime, endTime);

                var result = await _shiftApiClient.CreateAsync(command);

                if (result.IsSuccess)
                    Messages.Success("Shift added");

                if (result.IsFailure)
                    Messages.OutputErrorMessage(result.Errors);
            }
            else
            {
                Messages.Cancelled($"Did not add shift to employee log");
            }

            Messages.PrintBlankLines(1);
            enterShift = UserInput.GetConfirmation("Enter another shift?");
        }
        return;
    }
}