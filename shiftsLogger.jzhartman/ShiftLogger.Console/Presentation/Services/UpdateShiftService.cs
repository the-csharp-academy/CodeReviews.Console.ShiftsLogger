using ShiftLogger.Application.Shifts.Commands.UpdateShift;
using ShiftLogger.Application.Shifts.Dtos;
using ShiftLogger.Console.ApiClients.Shifts;
using ShiftLogger.Console.Presentation.Models;
using ShiftLogger.Console.Presentation.Output;
using ShiftLogger.Domain.Validation.Errors;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Services;

internal class UpdateShiftService
{
    private readonly IShiftApiClient _shiftApiClient;

    public UpdateShiftService(IShiftApiClient shiftApiClient)
    {
        _shiftApiClient = shiftApiClient;
    }

    public async Task RunAsync(EmployeeViewModel employee, ShiftDto shift)
    {
        var continueUpdate = true;

        while (continueUpdate)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"Updating Shift for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]");
            Messages.PrintBlankLines(2);

            PrintTime(shift.ClockInTime);
            var newClockInTime = UserInput.GetUpdatedTimeFromUser("Enter new [yellow]Clock-In Time[/]: ");
            Messages.PrintBlankLines(1);
            PrintTime(shift.ClockOutTime);
            var newClockOutTime = UserInput.GetUpdatedTimeFromUser("Enter new [yellow]Clock-Out Time[/]: ");

            Messages.PrintBlankLines(1);

            if (newClockInTime == DateTime.MinValue)
                newClockInTime = shift.ClockInTime;

            if (newClockOutTime == DateTime.MinValue)
                newClockOutTime = shift.ClockOutTime;

            if (newClockOutTime < newClockInTime)
            {
                Messages.OutputErrorMessage(Errors.ClockInTimePrecedesClockOutTime);
                continueUpdate = false;
                Messages.PressAnyKeyToContinue();
                continue;
            }

            var message = BuildUpdateConfirmationMessage(shift.ClockInTime, newClockInTime, shift.ClockOutTime, newClockOutTime);
            var confirmUpdate = UserInput.GetConfirmation(message);
            Messages.PrintBlankLines(1);

            if (confirmUpdate)
            {
                var command = new UpdateShiftCommand(shift.Id, shift.EmployeeId, newClockInTime, newClockOutTime);
                var result = await _shiftApiClient.UpdateAsync(command);

                if (result.IsSuccess)
                {
                    Messages.Success($"Updated shift");
                    Messages.PrintBlankLines(1);
                    continueUpdate = false;
                    Messages.PressAnyKeyToContinue();
                    continue;
                }

                if (result.IsFailure)
                {
                    Messages.OutputErrorMessage(result.Errors);
                    Messages.PrintBlankLines(1);
                }
            }
            else
            {
                Messages.Cancelled("Current shift not updated.");
                Messages.PrintBlankLines(1);
            }

            continueUpdate = UserInput.GetConfirmation("Retry updating shift?");
        }
    }

    private string BuildUpdateConfirmationMessage(DateTime originalClockInTime, DateTime newClockInTime,
                                                    DateTime originalClockOutTime, DateTime newClockOutTime)
    {
        var dateFormat = "yyyy-MM-dd HH:mm:ss";

        var clockInTimeMessage = (originalClockInTime == newClockInTime) ?
            "[green]No Changes[/]" :
            $"[yellow]{originalClockInTime.ToString(dateFormat)}[/]\tchanged to\t[green]{newClockInTime.ToString(dateFormat)}[/]";

        var clockOutTimeMessage = (originalClockOutTime == newClockOutTime) ?
            "[green]No Changes[/]" :
            $"[yellow]{originalClockOutTime.ToString(dateFormat)}[/]\tchanged to\t[green]{newClockOutTime.ToString(dateFormat)}[/]";

        var durationMessage = ((originalClockInTime == newClockInTime) && (originalClockOutTime == newClockOutTime)) ?
            "[green]No Changes[/]" :
            $"[yellow]{Messages.GenerateDurationString(originalClockInTime, originalClockOutTime)}[/]\t\tchanged to" +
            $"\t[green]{Messages.GenerateDurationString(newClockInTime, newClockOutTime)}[/]";


        return $"The following changes will be applied to the selected shift:" +
                $"\r\n\tClock-In Time:\t{clockInTimeMessage}" +
                $"\r\n\tClock-Out Time:\t{clockOutTimeMessage}" +
                $"\r\n\tDuration:\t{durationMessage}\t" +
                $"\r\n\r\nConfirm changes:";
    }
    private void PrintTime(DateTime time)
    {
        string parameterColor = "yellow";
        string valueColor = "green";

        AnsiConsole.MarkupLine($"Current [{parameterColor}]Clock-In Time[/]: [{valueColor}]{time.ToString("yyyy-MM-dd HH:mm:ss")}[/]");
    }
}