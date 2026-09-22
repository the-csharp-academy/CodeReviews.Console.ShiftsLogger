using ShiftLogger.Application.Shifts.Commands.DeleteShift;
using ShiftLogger.Application.Shifts.Dtos;
using ShiftLogger.Console.ApiClients.Shifts;
using ShiftLogger.Console.Presentation.Models;
using ShiftLogger.Console.Presentation.Output;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Services;

internal class DeleteShiftService
{
    private readonly IShiftApiClient _shiftApiClient;

    public DeleteShiftService(IShiftApiClient shiftApiClient)
    {
        _shiftApiClient = shiftApiClient;
    }

    public async Task RunAsync(EmployeeViewModel employee, ShiftDto shift)
    {
        var returnToMenu = false;

        while (returnToMenu == false)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"Deleting Shift for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]");
            Messages.PrintBlankLines(2);

            var confirmDelete = UserInput.GetConfirmation($"The following shift will be deleted:" +
                            $"\r\n\tClock-In Time:\t[green]{shift.ClockInTime.ToString("yyyy-MM-dd HH:mm:ss")}[/]" +
                            $"\r\n\tClock-Out Time:\t[green]{shift.ClockOutTime.ToString("yyyy-MM-dd HH:mm:ss")}[/]" +
                            $"\r\n\tDuration:\t[green]{Messages.GenerateDurationString(shift.ClockInTime, shift.ClockOutTime)}[/]" +
                            $"\r\n\r\nConfirm changes:");
            Messages.PrintBlankLines(1);

            if (confirmDelete)
            {
                var result = await _shiftApiClient.DeleteAsync(new DeleteShiftCommand(shift.Id,
                                                                                        shift.EmployeeId,
                                                                                        shift.ClockInTime,
                                                                                        shift.ClockOutTime));

                if (result.IsSuccess)
                {
                    Messages.Success($"Deleted shift!");
                    Messages.PrintBlankLines(1);
                    Messages.PressAnyKeyToContinue();
                    returnToMenu = true;
                }

                if (result.IsFailure)
                {
                    Messages.OutputErrorMessage(result.Errors);
                    Messages.PrintBlankLines(2);
                    returnToMenu = !UserInput.GetConfirmation("Retry delete?");
                }
            }
            else
            {
                Messages.Cancelled("Shift not deleted.");
                returnToMenu = true;
            }
        }
    }
}