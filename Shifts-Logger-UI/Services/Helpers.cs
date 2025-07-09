using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Shifts_Logger_UI.Controllers;
using Shifts_Logger_UI.Models;
using Shifts_Logger_UI.Utils;
using Shifts_Logger_UI.Views;
using Spectre.Console;
using Validator = Shifts_Logger_UI.Utils.Validator;

namespace Shifts_Logger_UI.Services;

public static class Helpers
{
    private static readonly ShiftsController Controller = new();
    private static readonly UserInterface Ui = new();
    private static readonly Menu Menu = new();

    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            ?.GetCustomAttribute<DisplayAttribute>()
            ?.Name;
    }

    internal static async Task<Shift> ChooseShift()
    {
        await Ui.ViewAllShifts();

        var shifts = await Controller.GetAllShiftsAsync(Controller.Client);

        if (shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Please create a shift to continue.[/]");
            await Menu.MainMenu();
        }

        else
        {

            var shiftArray = shifts.Select(s => s.Id).ToArray();

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Select an employee to view shift details:")
                    .AddChoices(shiftArray)
            );

            var selectedShift = shifts.FirstOrDefault(s => s.Id == userChoice);

            if (selectedShift == null)
            {
                AnsiConsole.MarkupLine("[red]Shift not found.[/]");
                return null;
            }

            return selectedShift;
        }

        return null;
    }

    internal static DateTime[] GetDates()
    {
        var startDateInput = AnsiConsole.Ask<string>("Enter Shift Start Time (yyyy-MM-dd HH:mm): ");

        while (!Validator.IsValidDate(startDateInput, "yyyy-MM-dd HH:mm"))
            startDateInput = AnsiConsole.Ask<string>(
                "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
            );

        var endDateInput = AnsiConsole.Ask<string>("Enter Shift End Time (yyyy-MM-dd HH:mm): ");

        while (!Validator.IsValidDate(endDateInput, "yyyy-MM-dd HH:mm"))
            endDateInput = AnsiConsole.Ask<string>(
                "\n[red]Invalid date. Format: yyyy-MM-dd HH:mm. Please try again:[/]\n"
            );

        while (!Validator.IsStartDateBeforeEndDate(startDateInput, endDateInput))
        {
            AnsiConsole.MarkupLine(
                "\n[red]Start date must be before end date. Please try again:[/]"
            );
            startDateInput = AnsiConsole.Ask<string>("Enter Shift Start Time (yyyy-MM-dd HH:mm): ");
            endDateInput = AnsiConsole.Ask<string>("Enter Shift End Time (yyyy-MM-dd HH:mm): ");
        }

        var startDate = DateTime.ParseExact(
            startDateInput,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
        var endDate = DateTime.ParseExact(
            endDateInput,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
        return [startDate, endDate];
    }
}
