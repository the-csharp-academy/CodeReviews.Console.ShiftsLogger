using silvermax.shiftlogger.UI.Models;
using Spectre.Console;

namespace silvermax.shiftlogger.UI;

internal class ShiftManager
{
    private readonly ShiftClient _client;

    public ShiftManager(ShiftClient shiftClient) => _client = shiftClient;

    public async Task<List<Shift>> GetAllShifts() => await _client.GetAllShifts();

    public async Task<Shift> CreateNewShift(CreateNewShiftDto dto) => await _client.CreateNewShift(dto);

    public async Task<string> DeleteShift(Guid id) => await _client.DeleteShift(id);

    public async Task<Shift> UpdateShift(Guid id, Shift updatedShift) => await _client.UpdateShift(id, updatedShift);

    public async Task DisplayAllShiftsInteractive()
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

    public async Task CreateNewShiftInteractive()
    {
        try
        {
            var dto = UserInput.GetNewShift();

            var response = await CreateNewShift(dto);

            AnsiConsole.MarkupLine($"[green]Shift created successfully: {response}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error creating shift: {ex.Message}[/]");
        }
        
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }

    public async Task DeleteshiftInteractive()
    {
        try
        {
            var shifts = await GetAllShifts();

            if (shifts.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No shifts found to delete.[/]");
                Console.ReadKey();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                .Title("What shift would you like to delete?:")
                .UseConverter(s => $"{s.name} - {s.startTime:yyyy-MM-dd HH:mm}")
                .AddChoices(shifts));

            var response = await DeleteShift(selected.shiftId);

            AnsiConsole.MarkupLine($"[green]{response}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error deleting shift: {ex.Message}[/]");
        }

        
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }

    public async Task UpdateShiftInteractive()
    {

        try
        {
            var shifts = await GetAllShifts();

            if (shifts.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No shifts found to update.[/]");
                Console.ReadKey();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<Shift>()
                .Title("What shift would you like to update?:")
                .UseConverter(s => $"{s.name} - {s.startTime:yyyy-MM-dd HH:mm}")
                .AddChoices(shifts));

            var newShift = UserInput.GetNewShift();

            var updatedshift = new Shift(
                selected.shiftId, newShift.Name, newShift.StartTime, newShift.EndTime, (newShift.EndTime - newShift.StartTime)); //duration gets ignored since it gets computed by db.

            var response = await UpdateShift(selected.shiftId, updatedshift);

            AnsiConsole.MarkupLine($"[green]Shift updated successfully: {response}[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error updating shift: {ex.Message}[/]");
        }
        
        AnsiConsole.MarkupLine("[grey]Press any key to return to the menu...[/]");
        Console.ReadKey();
    }
}
