using ShiftsLogger.UI.DTOs;
using ShiftsLogger.UI.Services;
using System;

namespace ShiftsLogger.UI
{
    public class WorkerInput
    {
        private readonly WorkerApiService _workerApiService;
        private readonly ShiftApiService _shiftApiService;

        private WorkerInput(WorkerApiService workerApiService, ShiftApiService shiftApiService)
        {
            _workerApiService = workerApiService;
            _shiftApiService = shiftApiService;
        }

        public static async Task RunAsync(WorkerApiService workerApiService, ShiftApiService shiftApiService)
        {
            var input = new WorkerInput(workerApiService, shiftApiService);
            await input.GetWorkerMenuInput();
        }

        private async Task GetWorkerMenuInput()
        {
            Console.Clear();
            List<string> choices = new();
            choices.Add("Exit");
            choices.Add("See all workers.");
            choices.Add("Search worker by ID.");
            choices.Add("Add new worker.");
            TableVisualizer.ShowChoices(choices, "WORKERS MENU");

            string? inp = Console.ReadLine();
            while (!Validator.IsValidChoice(inp, 3))
            {
                Console.Write("\nPlease enter a valid number!\nTry again:\n> ");
                inp = Console.ReadLine();
            }
            int choice = int.Parse(inp!);

            switch (choice)
            {
                case 0:
                    Console.WriteLine("\nGood bye!");
                    break;
                case 1:
                    await GetAllWorkerInput();
                    break;
                case 2:
                    await GetWorkerByIdInput();
                    break;
                case 3:
                    await AddWorkerInput();
                    break;
                default:
                    break;
            }
        }

        private async Task GetAllWorkerInput()
        {
            try
            {
                var workers = await _workerApiService.GetAllWorkersAsync();

                if (workers == null || workers.Count == 0)
                {
                    Console.WriteLine("\nNo worker found!");
                    return;
                }

                TableVisualizer.ShowTable<WorkerReadDTO>(workers, "WORKERS LIST");

                await GetWorkerByIdInput();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }

        private async Task GetWorkerByIdInput()
        {
            try
            {
                var workers = await _workerApiService.GetAllWorkersAsync();

                if (workers == null || workers.Count == 0)
                {
                    Console.WriteLine("\nNo workers found!");
                    return;
                }

                var worker = InputHelper.ChooseWorker(workers);
                if (worker == null)
                {
                    Console.WriteLine("\nNo worker found!");
                    return;
                }

                await GetWorkerActionMenuInput(worker);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }

        private async Task AddWorkerInput()
        {
            try
            {
                Console.WriteLine("\nCREATE NEW WORKER");
                Console.Write("Enter name:\t");
                string? inp = Console.ReadLine();
                while (!Validator.IsValidString(inp))
                {
                    Console.WriteLine("\nPlease enter a valid name or 0 to exit.");
                    Console.Write("Try again:\t");
                    inp = Console.ReadLine();
                    if (inp == "0")
                    {
                        return;
                    }
                }

                var res = await _workerApiService.CreateWorkerAsync(new WorkerDTO { Name = inp! });
                if (res != null)
                {
                    Console.WriteLine($"\nWorker {res.Name} created successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }

        private async Task GetWorkerActionMenuInput(WorkerReadDTO worker)
        {
            List<WorkerReadDTO> list = new();
            list.Add(worker);
            TableVisualizer.ShowTable(list, worker.Name);

            List<string> choices = new();
            choices.Add("Exit");
            choices.Add("Update worker's name.");
            choices.Add("Delete worker.");
            choices.Add("See all worker's shifts.");
            TableVisualizer.ShowChoices(choices, "WORKER ACTION MENU");

            string? inp = Console.ReadLine();
            while (!Validator.IsValidChoice(inp, 3))
            {
                Console.Write("\nPlease enter a valid number!\nTry again:\n> ");
                inp = Console.ReadLine();
            }
            int choice = int.Parse(inp!);

            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    await UpdateWorkerInput(worker);
                    break;
                case 2:
                    await DeleteWorkerInput(worker);
                    break;
                case 3:
                    await GetShiftsInput(worker);
                    break;
                default:
                    break;
            }
        }

        private async Task UpdateWorkerInput(WorkerReadDTO worker)
        {
            try
            {
                Console.WriteLine("\nUPDATE WORKER NAME");
                Console.Write("Enter new name:\t");
                string? inp = Console.ReadLine();
                while (!Validator.IsValidString(inp))
                {
                    Console.WriteLine("\nPlease enter a valid name or 0 to exit.");
                    Console.Write("Try again:\t");
                    inp = Console.ReadLine();
                    if (inp == "0")
                    {
                        return;
                    }
                }

                var res = await _workerApiService.UpdateWorkerAsync(worker.Id, new WorkerDTO { Name = inp! });
                if (res)
                {
                    Console.WriteLine($"\nWorker {inp} updated successfully!");
                }
                else
                {
                    Console.WriteLine($"\nError occurred while updating worker {worker.Name}!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }

        private async Task DeleteWorkerInput(WorkerReadDTO worker)
        {
            try
            {
                var res = await _workerApiService.DeleteWorkerAsync(worker.Id);
                if (res)
                {
                    Console.WriteLine($"\nWorker {worker.Name} deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"\nError occurred while deleting worker {worker.Name}!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }

        private async Task GetShiftsInput(WorkerReadDTO worker)
        {
            try
            {
                var shifts = await _shiftApiService.GetShiftsByWorkerAsync(worker.Id);

                if (shifts == null || shifts.Count == 0)
                {
                    Console.WriteLine("\nNo shift found!");
                    return;
                }

                TableVisualizer.ShowTable(shifts, $"LIST OF SHIFTS BY {worker.Name.ToUpper()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nUnexpected error: {ex.Message}");
                return;
            }
        }
    }
}
