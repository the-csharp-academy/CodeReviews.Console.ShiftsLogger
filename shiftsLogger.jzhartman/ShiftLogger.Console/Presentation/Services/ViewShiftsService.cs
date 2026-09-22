using ShiftLogger.Application.Shifts.Dtos;
using ShiftLogger.Console.ApiClients.Shifts;
using ShiftLogger.Console.Presentation.Enums;
using ShiftLogger.Console.Presentation.Models;
using ShiftLogger.Console.Presentation.Output;
using ShiftLogger.Console.Presentation.Views;
using ShiftLogger.Console.Presentation.Views.Menus;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Services;

internal class ViewShiftsService
{
    private readonly IShiftApiClient _shiftApiClient;
    private readonly ShiftTableView _shiftTableView;
    private readonly ShiftMenuView _shiftMenuView;
    private readonly UpdateShiftService _updateShiftService;
    private readonly DeleteShiftService _deleteShiftService;

    public ViewShiftsService(IShiftApiClient shiftApiClient, ShiftTableView shiftTableView, ShiftMenuView shiftMenuView,
                            UpdateShiftService updateShiftService, DeleteShiftService deleteShiftService)
    {
        _shiftApiClient = shiftApiClient;
        _shiftTableView = shiftTableView;
        _shiftMenuView = shiftMenuView;
        _updateShiftService = updateShiftService;
        _deleteShiftService = deleteShiftService;
    }

    public async Task RunAsync(EmployeeViewModel employee)
    {
        bool returnToEmployeeMenu = false;

        while (returnToEmployeeMenu == false)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"Viewing shifts for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]");
            Messages.PrintBlankLines(2);


            var startDate = UserInput.GetTimeFromUser("Enter start date (Format: [yellow]yyyy-MM-dd[/]): ", true);
            var endDate = UserInput.GetTimeFromUser("Enter end date (Format: [yellow]yyyy-MM-dd[/]): ", true);
            endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            bool newDateRange = false;
            while (newDateRange == false)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"Viewing shifts for [DeepSkyBlue1]{employee.FirstName} {employee.LastName}[/]");
                Messages.PrintBlankLines(2);

                var result = await _shiftApiClient.GetByDateRangeAndIdAsync(new(employee.Id, employee.FirstName, employee.LastName),
                                                                            startDate,
                                                                            endDate);

                if (result.IsSuccess)
                {
                    _shiftTableView.Render(result.Value);

                    Messages.PrintBlankLines(1);
                    PrintDurationForRange(CalculateShiftDurationForRange(result.Value));
                    Messages.PrintBlankLines(2);

                    var menuOptions = Enum.GetValues<ShiftMenuItem>().ToArray();

                    if (result.Value is null || result.Value.Count == 0)
                    {
                        ShiftMenuItem[] unusedItems = { ShiftMenuItem.EditShift, ShiftMenuItem.DeleteShift };
                        menuOptions = menuOptions.Except(unusedItems).ToArray();
                    }

                    var menuSelection = _shiftMenuView.Render(menuOptions);

                    switch (menuSelection)
                    {
                        case ShiftMenuItem.EditShift:
                            var shift = GetShiftFromUser(result.Value, "edit");
                            await _updateShiftService.RunAsync(employee, shift);
                            break;
                        case ShiftMenuItem.DeleteShift:
                            shift = GetShiftFromUser(result.Value, "edit");
                            await _deleteShiftService.RunAsync(employee, shift);
                            break;
                        case ShiftMenuItem.SelectNewDateRange:
                            newDateRange = true;
                            break;
                        case ShiftMenuItem.Return:
                            newDateRange = true;
                            returnToEmployeeMenu = true;
                            break;
                        default:
                            AnsiConsole.MarkupLine("[red]ERROR:[/] Unknown input for main menu selection!");
                            break;
                    }

                    if (result.IsFailure)
                        Messages.OutputErrorMessage(result.Errors);
                }
            }
        }
        return;
    }

    private ShiftDto GetShiftFromUser(List<ShiftDto> shifts, string action)
    {
        var message = $"Enter the row [yellow]Id[/] of the shift you would like to {action}:";
        var index = UserInput.GetNumberFromUser(message, shifts.Count) - 1;

        return shifts[index];
    }

    private double CalculateShiftDurationForRange(List<ShiftDto> shifts)
    {
        double duration = 0;

        foreach (var shift in shifts)
        {
            duration += (shift.ClockOutTime - shift.ClockInTime).TotalSeconds;
        }

        return duration;
    }
    private void PrintDurationForRange(double durationSeconds)
    {
        var duration = TimeSpan.FromSeconds(durationSeconds);

        AnsiConsole.MarkupLine($"[DeepSkyBlue1]Total Duration for Range (HH:mm:ss)[/]: [green]" +
            $"{(int)Math.Floor(duration.TotalHours):D2}:{duration.Minutes:D2}:{duration.Seconds:D2}[/]");
    }
}