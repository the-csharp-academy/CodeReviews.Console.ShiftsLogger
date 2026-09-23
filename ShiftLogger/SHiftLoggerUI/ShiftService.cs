using System.Net.Http.Json;

namespace ShiftLoggerUI;

static class ShiftService
{
    static HttpClient client = new HttpClient();

    public static async Task GetShifts()
    {
        try
        {
            var shifts = await client.GetFromJsonAsync<List<Shift>>("http://localhost:5011/api/Shifts");

            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("There are no shifts yet.");
                return;
            }

            foreach (var shift in shifts)
            {
                Console.WriteLine($"Worker - ID: {shift.Id}, Worker ID: {shift.WorkerId}, Hours: {shift.StartTime} - {shift.EndTime}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task GetShift()
    {
        int id = UserInput.GetId("Please insert the ID of the shift you want to get: ");
        try
        {
            var shift = await client.GetFromJsonAsync<Shift>($"http://localhost:5011/api/Shifts/{id}");
            Console.WriteLine($"Id: {shift!.Id}, Name: {shift.WorkerId}, Hours: {shift.StartTime} - {shift.EndTime}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task AddShift()
    {
        int workerId = UserInput.GetId("Please insert the ID of the worker: ");

        DateTime start = UserInput.GetDateTime("Please enter the date and time of the shift start - format [dd-MM-yyyy] - [hh:mm:ss] ");
        DateTime end = UserInput.GetDateTime("Please enter the date and time of the shift end - format [dd-MM-yyyy] - [hh:mm:ss] ");

        Shift shift = new Shift { StartTime = start, EndTime = end, WorkerId = workerId };
        try
        {
            var response = await client.PostAsJsonAsync<Shift>("http://localhost:5011/api/Shifts", shift);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Shift added successfully!");

                var newShift = await response.Content.ReadFromJsonAsync<Shift>();

                Console.WriteLine($"Id: {newShift!.Id}, Name: {newShift.WorkerId}, Hours: {newShift.StartTime} - {newShift.EndTime}");
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to add shift. Status: {response.StatusCode}. Details: {errorBody}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task EditShift()
    {
        await GetShifts();

        int id = UserInput.GetId("Insert ID of the shift you want to edit: ");

        try
        {
            var existingShift = await client.GetFromJsonAsync<Shift>($"http://localhost:5011/api/Shifts/{id}");

            Console.WriteLine($"You are now modifying shift ID {existingShift!.Id}: WORKER: {existingShift.WorkerId}, {existingShift.StartTime} - {existingShift.EndTime} ");

            DateTime start = UserInput.GetDateTime("Insert new starting time: ");

            DateTime end = UserInput.GetDateTime("Insert new ending time: ");

            Shift shift = new Shift { Id = id, StartTime = start, EndTime = end, WorkerId = existingShift.WorkerId };

            var response = await client.PutAsJsonAsync<Shift>($"http://localhost:5011/api/Shifts/{id}", shift);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Shift updated correctly!");
            }
            else
            {
                Console.WriteLine("Error updating the shift.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task DeleteShift()
    {
        await GetShifts();

        int id = UserInput.GetId("Select shift to delete: ");

        try
        {
            var response = await client.DeleteAsync($"http://localhost:5011/api/Shifts/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Shift deleted successfully. ");
            }
            else
            {
                Console.WriteLine("Error deleting the shift. ");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
