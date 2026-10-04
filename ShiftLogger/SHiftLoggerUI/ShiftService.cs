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
                Console.WriteLine($"Worker - ID: {shift.Id}, Worker ID: {shift.WorkerId}, Hours: {shift.StartTime} - {shift.EndTime}, Duration: {shift.Duration()}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task GetShift()
    {
        try
        {
            int id = await GetExistingShiftId("Please insert the ID of the shift you want to get: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }
            
            var shift = await client.GetFromJsonAsync<Shift>($"http://localhost:5011/api/Shifts/{id}");
            Console.WriteLine($"Id: {shift!.Id}, worker ID: {shift.WorkerId}, Hours: {shift.StartTime} - {shift.EndTime}, Duration: {shift.Duration()}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task AddShift()
    {
        try
        {
            int workerId = await WorkerService.GetExistingWorkerId("Please insert the ID of the worker you want to add a shift for: ");
            if (workerId == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }
            (DateTime start, DateTime end) = UserInput.GetDateTimes("Please enter the date and time of the shift start - format [dd-MM-yyyy] - [HH:mm:ss] ");

            Shift shift = new Shift { StartTime = start, EndTime = end, WorkerId = workerId };
            var response = await client.PostAsJsonAsync<Shift>("http://localhost:5011/api/Shifts", shift);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Shift added successfully!");

                var newShift = await response.Content.ReadFromJsonAsync<Shift>();

                Console.WriteLine($"Id: {newShift!.Id}, Worker ID: {newShift.WorkerId}, Hours: {newShift.StartTime} - {newShift.EndTime}, Duration: {newShift.Duration()}");
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
        try
        {
            await GetShifts();

            int id = await GetExistingShiftId("Please insert the ID of the shift you want to edit: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            var existingShift = await client.GetFromJsonAsync<Shift>($"http://localhost:5011/api/Shifts/{id}");

            Console.WriteLine($"You are now modifying shift ID {existingShift!.Id}: WORKER: {existingShift.WorkerId}, {existingShift.StartTime} - {existingShift.EndTime}, Duration: {existingShift.Duration()}");

            (DateTime start, DateTime end) = UserInput.GetDateTimes("Insert new starting time - format [dd-MM-yyyy] - [HH:mm:ss] ");

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
        try
        {
            await GetShifts();

            int id = await GetExistingShiftId("Please insert the ID of the shift you want to delete: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

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

    public static async Task<int> GetExistingShiftId(string message)
    {
        while (true)
        {
            int id = UserInput.GetId(message);
            if(id == 0)
            {
                return 0;
            }

            if (await ShiftExists(id))
            {
                return id;
            }
            Console.WriteLine("Shift with this ID does not exist. Please try again.");
        }
    }
    public static async Task<bool> ShiftExists(int id)
    {
        var response = await client.GetAsync($"http://localhost:5011/api/Shifts/{id}");
        return response.IsSuccessStatusCode;
    }
}
