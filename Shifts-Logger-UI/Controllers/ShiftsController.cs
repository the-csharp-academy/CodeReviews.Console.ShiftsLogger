using System.Net.Http.Json;
using Newtonsoft.Json;
using Shifts_Logger_UI.Models;
using Shifts_Logger_UI.Services;
using Spectre.Console;

namespace Shifts_Logger_UI.Controllers;

public class ShiftsController
{
    internal HttpClient Client = new() { BaseAddress = new Uri("http://localhost:5267") };

    internal async Task<List<Shift>> GetAllShiftsAsync(HttpClient client)
    {
        try
        {
            using var response = await client.GetAsync("/api/shift");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<List<Shift>>(jsonResponse);

            var shifts = deserializedResponse;

            return shifts;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            throw;
        }
    }

    internal async Task CreateShiftAsync(HttpClient client)
    {
        try
        {
            var dates = Helpers.GetDates();
            var newShift = new Shift
            {
                EmployeeName = AnsiConsole.Ask<string>("Enter Employee Name: "),
                Position = AnsiConsole.Ask<string>("Enter Position: "),
                ShiftNumber = AnsiConsole.Ask<int>("Enter Shift Number (1, 2 or 3): "),
                StartTime = dates[0],
                EndTime = dates[1],
            };

            using var response = await client.PostAsJsonAsync("/api/shift", newShift);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            JsonConvert.SerializeObject(jsonResponse);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            throw;
        }
    }

    internal async Task UpdateShift(HttpClient client)
    {
        try
        {
            var shift = await Helpers.ChooseShift();
            var id = shift.Id;

            var dates = Helpers.GetDates();

            var updatedShift = new Shift
            {
                EmployeeName = AnsiConsole.Ask<string>("Enter Employee Name: "),
                Position = AnsiConsole.Ask<string>("Enter Position: "),
                ShiftNumber = AnsiConsole.Ask<int>("Enter Shift Number (1, 2 or 3): "),
                StartTime = dates[0],
                EndTime = dates[1],
            };

            using var response = await client.PutAsJsonAsync($"/api/shift/{id}", updatedShift);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            JsonConvert.DeserializeObject<Shift>(jsonResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    internal async Task DeleteShift(HttpClient client)
    {
        try
        {
            var shift = await Helpers.ChooseShift();
            var id = shift.Id;

            using var response = await client.DeleteAsync($"/api/shift/{id}");

            response.EnsureSuccessStatusCode();

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[green]Shift deleted successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine(ex.Message);
            throw;
        }
    }
}
