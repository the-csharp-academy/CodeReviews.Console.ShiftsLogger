using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class WorkerController
{
    public static async Task ViewWorkers(string heading, int? workerId = null)
    {
        var workerService = new WorkerService(new HttpClient());
        List<WorkerDto> viewWorkers = new List<WorkerDto>();

        if (workerId.HasValue)
        {
            try
            {
                var worker = await workerService.GetWorkerAsync(workerId.Value);
                viewWorkers = worker is not null ? new List<WorkerDto> { worker } : new List<WorkerDto>();
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                Console.WriteLine($"\nFailed to retrieve worker with ID {workerId.Value}. Request error: {e.Message}");
            }
        }
        else
        {
            try
            {
                viewWorkers = await workerService.GetWorkersAsync();
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                Console.WriteLine("\nFailed to retrieve workers. Request error: " + e.Message);
            }
        }

        Display.PrintWorkersTable(viewWorkers, heading);
    }

    public static async Task CreateWorker()
    {
        try
        {
            await ViewWorkers("Add Worker");
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve workers before creating a new one. Request error: {e.Message}");
            return; // Exit early if workers can't be retrieved
        }
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());
        var newFirstName = userInterface.ReadString("\nEnter First Name: ");
        var newLastName = userInterface.ReadString("Enter Last Name: ");
        var newTitle = userInterface.ReadString("Enter a Title: ");
        var worker = new WorkerDto { Title = newTitle, FirstName = newFirstName, LastName = newLastName };
        try
        {
            var insertResult = await workerService.InsertWorker(worker);

            Console.Clear();
            try
            {
                await ViewWorkers("Add Worker");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nFailed to refresh worker list after creation. Request error: {e.Message}");
            }

            if (insertResult)
            {
                Console.WriteLine("\nSuccessfully added worker!");
            }
            else
            {
                Console.WriteLine("\nFailed to add worker. No exception was thrown, but the request was unsuccessful.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            try
            {
                await ViewWorkers("Add Worker");
            }
            catch (HttpRequestException viewError)
            {
                Console.WriteLine($"\nFailed to refresh worker list after failed creation. Request error: {viewError.Message}");
            }

            Console.WriteLine($"\nFailed to add worker. Request error: {e.Message}");
        }
    }

    public static async Task EditWorker()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());

        List<WorkerDto> editWorkers = new List<WorkerDto>();
        try
        {
            editWorkers = await workerService.GetWorkersAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve workers. Request error: {e.Message}");
            return; // Exit early if workers can't be retrieved
        }
        var rule = new Rule("[green]Edit Worker[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);
        
        var editWorker = userInterface.SelectWorker("\nChoose Worker to Edit:", editWorkers);
        if (editWorker.FirstName == null)
        {
            return;
        }

        try
        {
            await ViewWorkers("Edit Worker", editWorker.WorkerId);
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve worker details. Request error: {e.Message}");
            return; // Exit early if worker details can't be retrieved
        }
        Console.WriteLine();
        editWorker.FirstName = AnsiConsole.Confirm("Update worker's first name?", false) ? AnsiConsole.Ask<string>("Worker's new first name:") : editWorker.FirstName;
        Console.WriteLine();
        editWorker.LastName = AnsiConsole.Confirm("Update worker's last name?", false) ? AnsiConsole.Ask<string>("Worker's new last name:") : editWorker.LastName;
        Console.WriteLine();
        editWorker.Title = AnsiConsole.Confirm("Update worker's title?", false) ? AnsiConsole.Ask<string>("Worker's new title:") : editWorker.Title;

        try
        {
            var editResult = await workerService.UpdateWorker(editWorker);
            Console.Clear();
            await ViewWorkers("Edit Worker");

            if (editResult)
            {
                Console.WriteLine("\nSuccessfully edited worker!");
            }
            else
            {
                Console.WriteLine("\nFailed to edit worker.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            await ViewWorkers("Edit Worker");
            Console.WriteLine($"\nFailed to edit worker. Request error: {e.Message}");
        }
    }

    public static async Task DeleteWorker()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var workerService = new WorkerService(new HttpClient());

        List<WorkerDto> deleteWorkers = new List<WorkerDto>();
        try
        {
            deleteWorkers = await workerService.GetWorkersAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve workers. Request error: {e.Message}");
            return; // Exit early if workers can't be retrieved
        }

        var rule = new Rule("[green]Delete Worker[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var deleteWorker = userInterface.SelectWorker("\nChoose Worker to Delete:", deleteWorkers);
        if (deleteWorker.FirstName == null)
        {
            return;
        }

        try
        {
            await ViewWorkers("Delete Worker", deleteWorker.WorkerId);
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve worker details. Request error: {e.Message}");
            return; // Exit early if shift details can't be retrieved
        }

        Console.WriteLine();

        if (AnsiConsole.Confirm("[yellow]Do you really want to delete this worker?[/]", false))
        {
            try
            {
                var deleteResult = await workerService.DeleteWorker(deleteWorker.WorkerId);
                Console.Clear();
                await ViewWorkers("Delete Worker");

                if (deleteResult)
                {
                    Console.WriteLine("\nSuccessfully deleted worker!");
                }
                else
                {
                    Console.WriteLine("\nFailed to delete worker.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                await ViewWorkers("Delete Worker");
                Console.WriteLine($"\nFailed to delete worker. Request error: {e.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nWorker not deleted.");
        }
    }
}
