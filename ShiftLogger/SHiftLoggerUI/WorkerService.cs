using System.Net.Http.Json;

namespace ShiftLoggerUI;

static class WorkerService
{
    static HttpClient client = new HttpClient();
    public static async Task GetWorkers()
    {
        try
        {
            var workers = await client.GetFromJsonAsync<List<Worker>>("http://localhost:5011/api/Workers");

            if (workers == null || workers.Count == 0)
            {
                Console.WriteLine("There are no workers yet.");
                return;
            }

            foreach (var worker in workers!)
            {
                Console.WriteLine($"Worker - Id: {worker.Id}, Name: {worker.Name}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task GetWorker()
    {
        try
        {
            int id = await WorkerService.GetExistingWorkerId("Please insert the ID of the worker you want to get: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }
            
            var worker = await client.GetFromJsonAsync<Worker>($"http://localhost:5011/api/Workers/{id}");
            Console.WriteLine($"Worker: {worker!.Name} ID: {worker.Id}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task GetWorkerShifts()
    {
        await GetWorkers();
        try
        {
            int id = await WorkerService.GetExistingWorkerId("Please insert the ID of the worker you want to get the shifts for: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            var shifts = await client.GetFromJsonAsync<List<Shift>>($"http://localhost:5011/api/Workers/{id}/Shifts");

            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("There are no shifts for this worker. ");
            }
            else
            {
                foreach (var shift in shifts)
                {
                    Console.WriteLine($"shift {shift.Id}: {shift.StartTime} - {shift.EndTime}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task AddWorker()
    {
        string name = UserInput.GetName("Please enter the name of the new worker: ");
        try
        {
            Worker worker = new Worker { Name = name };

            var response = await client.PostAsJsonAsync<Worker>("http://localhost:5011/api/Workers", worker);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Worker added successfully!");

                var newWorker = await response.Content.ReadFromJsonAsync<Worker>();

                Console.WriteLine($"{newWorker!.Id} - {newWorker.Name}");
            }else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to add worker. Status: {response.StatusCode}. Details: {errorBody}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task EditWorker()
    {
        try
        {
            await GetWorkers();

            int id = await WorkerService.GetExistingWorkerId("Please insert the ID of the worker you want to edit: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            string name = UserInput.GetName("Please enter the updated name of the worker: ");

            Worker worker = new Worker { Name = name, Id = id };


            var response = await client.PutAsJsonAsync($"http://localhost:5011/api/Workers/{id}", worker);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Worker updated correctly!");
            }
            else
            {
                Console.WriteLine("Error updating the worker.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task DeleteWorker()
    {
        await GetWorkers();
        try
        {
            int id = await WorkerService.GetExistingWorkerId("Please insert the ID of the worker you want to delete: ");
            if (id == 0)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            var response = await client.DeleteAsync($"http://localhost:5011/api/Workers/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Worker deleted successfully. ");
            }
            else
            {
                Console.WriteLine("Error deleting the worker. ");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    public static async Task<int> GetExistingWorkerId(string message)
    {
        while (true)
        {
            int id = UserInput.GetId(message);
            if(id == 0)
            {
                return 0;
            }

            if (await WorkerExists(id))
            {
                return id;
            }
            Console.WriteLine($"Worker with ID {id} does not exist. Please try again.");
        }

    }

    public static async Task<bool> WorkerExists(int id)
    {
        var response = await client.GetAsync($"http://localhost:5011/api/Workers/{id}");
        return response.IsSuccessStatusCode;
    }
}