using Microsoft.Extensions.DependencyInjection;
using silvermax.shiftlogger.UI.Models;
using Spectre.Console;
using static silvermax.shiftlogger.UI.Enums;

namespace silvermax.shiftlogger.UI;

internal class UserInterface
{
    public async Task Start()
    {
        bool openManager = true;

        while (openManager)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("Welcome to the Shift Manager.")
                .UseConverter(e => Enums.DisplayChoice(e))
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (choice)
            {
                case MenuOptions.AddShift:
                    await CreateNewShift();
                    break;

                case MenuOptions.UpdateShift:
                    await UpdateShift();
                    break;

                case MenuOptions.SeeAllShifts:
                    await DisplayAllShifts();
                    break;

                case MenuOptions.DeleteShift:
                    await DeleteShift();
                    break;

                case MenuOptions.Exit:
                    openManager = false;
                    break;
            }
        }
    }

    private static ShiftManager GetManager()
    {
        var services = new ServiceCollection();
        var serviceProvider = services.ServiceProvider();

        return serviceProvider.GetRequiredService<ShiftManager>();
    }

    private static async Task<List<Shift>> GetAllShifts()
    {
        return await GetManager().GetAllShifts();
    }

    private static async Task DisplayAllShifts()
    {
        try
        {
            var shifts = await GetAllShifts();

            if (shifts is null)
            {
                AnsiConsole.MarkupLine("[red]Failed to retrieve shifts (null).[/]");
                Console.ReadKey();
                return;
            }

            if (shifts.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No shifts found.[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table { Border = TableBorder.Rounded };
            table.AddColumn("[yellow]Id[/]");
            table.AddColumn("[yellow]Name[/]");
            table.AddColumn("[yellow]Start Time[/]");
            table.AddColumn("[yellow]End Time[/]");
            table.AddColumn("[yellow]Duration[/]");

            foreach (var s in shifts)
            {
                table.AddRow(
                    s.shiftId.ToString(),
                    $"[purple]{s.name}[/]",
                    $"[purple]{s.startTime:yyyy-MM-dd HH:mm:ss}[/]",
                    $"[purple]{s.endTime:yyyy-MM-dd HH:mm:ss}[/]",
                    $"[purple]{s.duration}[/]"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error retrieving shifts: {ex.Message}[/]");
            Console.ReadKey();
        }
    }

    public static async Task CreateNewShift()
    {
        var dto = UserInput.GetNewShift();

        var response = await GetManager().CreateNewShift(dto);

        AnsiConsole.MarkupLine($"[green]Shift created successfully: {response}[/]");
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }

    public static async Task DeleteShift()
    {
        var shifts = await GetAllShifts();

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Shift>()
            .Title("What shift would you like to delete?:")
            .UseConverter(s => $"{s.name} - {s.startTime:yyyy-MM-dd HH:mm}")
            .AddChoices(shifts));

        var response = await GetManager().DeleteShift(selected.shiftId);

        AnsiConsole.MarkupLine($"[green]{response}[/]");
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }

    public async Task UpdateShift()
    {
        var shifts = await GetAllShifts();

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Shift>()
            .Title("What shift would you like to delete?:")
            .UseConverter(s => $"{s.name} - {s.startTime:yyyy-MM-dd HH:mm}")
            .AddChoices(shifts));

        var newShift = UserInput.GetNewShift();

        var updatedshift = new Shift(
            selected.shiftId, newShift.Name, newShift.StartTime, newShift.EndTime, (newShift.EndTime - newShift.StartTime)); //duration gets ignored since it gets computed by db.

        var response = await GetManager().UpdateShift(selected.shiftId, updatedshift);

        AnsiConsole.MarkupLine($"[green]Shift updated successfully: {response}[/]");
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }
}
