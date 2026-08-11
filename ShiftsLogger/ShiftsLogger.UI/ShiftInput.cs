using ShiftsLogger.UI.DTOs;
using ShiftsLogger.UI.Services;
using System;
using System.Globalization;

namespace ShiftsLogger.UI;

public class ShiftInput
{
    private readonly ShiftApiService _shiftApiService;
    private readonly WorkerApiService _workerApiService;

    private ShiftInput(WorkerApiService workerApiService, ShiftApiService shiftApiService)
    {
        _shiftApiService = shiftApiService;
        _workerApiService = workerApiService;
    }

    public static async Task RunAsync(WorkerApiService workerApiService, ShiftApiService shiftApiService)
    {
        var input = new ShiftInput(workerApiService, shiftApiService);
        await input.GetShiftMenuInput();
    }

    private async Task GetShiftMenuInput()
    {
        Console.Clear();
        List<string> choices = new();
        choices.Add("Exit");
        choices.Add("See all shifts.");
        choices.Add("Search shift by ID.");
        choices.Add("Add new shift.");
        TableVisualizer.ShowChoices(choices, "SHIFTS MENU");

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
                await GetAllShiftsInput();
                break;
            case 2:
                await GetShiftByIdInput();
                break;
            case 3:
                await AddShiftInput();
                break;
            default:
                break;
        }
    }

    private async Task GetAllShiftsInput()
    {
        try
        {
            var shifts = await _shiftApiService.GetAllShiftsAsync();

            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("\nNo shift found!");
                return;
            }

            TableVisualizer.ShowTable<ShiftReadDTO>(shifts, "SHIFTS LIST");

            await GetShiftByIdInput();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private async Task GetShiftByIdInput()
    {
        try
        {
            var shifts = await _shiftApiService.GetAllShiftsAsync();

            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("\nNo shifts found!");
                return;
            }

            var shift = ChooseShiftInput(shifts);
            if (shift == null)
            {
                Console.WriteLine("\nNo shift found!");
                return;
            }

            await GetShiftActionInput(shift);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private async Task GetShiftActionInput(ShiftReadDTO shift)
    {
        List<ShiftReadDTO> list = new();
        list.Add(shift);
        TableVisualizer.ShowTable(list, "SHIFT INFO");

        List<string> choices = new();
        choices.Add("Exit");
        choices.Add("Update shift's time.");
        choices.Add("Update shift's worker.");
        choices.Add("Delete shift.");
        TableVisualizer.ShowChoices(choices, "SHIFT ACTION MENU");

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
                await UpdateShiftTimeInput(shift);
                break;
            case 2:
                await UpdateShiftWorkerInput(shift);
                break;
            case 3:
                await DeleteShiftInput(shift);
                break;
            default:
                break;
        }
    }

    private async Task AddShiftInput()
    {
        Console.WriteLine("ADD NEW SHIFT");

        var worker = await ChooseWorkerInput();
        if (worker == null)
        {
            return;
        }

        string format = "dd/MM/yyyy HH:mm:ss";

        Console.WriteLine($"Please choose starting time with exact format ({format})");
        Console.Write("Your choice:\t");
        string? inp = Console.ReadLine();

        bool isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime);
        while (!isSuccess)
        {
            Console.WriteLine($"Please enter a valid time with format ({format}) or 0 to exit");
            Console.Write("Try again:\t");
            inp = Console.ReadLine();

            if (inp == "0")
            {
                return;
            }

            isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime);
        }

        bool validEndTime = false;
        DateTime endTime = DateTime.MinValue;

        while (!validEndTime)
        {
            Console.WriteLine($"Please choose ending time with exact format ({format})");
            Console.Write("Your choice:\t");
            inp = Console.ReadLine();

            isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime);
            while (!isSuccess)
            {
                Console.WriteLine($"Please enter a valid time with format ({format}) or 0 to exit");
                Console.Write("Try again:\t");
                inp = Console.ReadLine();

                if (inp == "0")
                {
                    return;
                }

                isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime);
            }

            if (endTime <= startTime)
            {
                Console.WriteLine("\nError: End time must be after start time! Please try again.");
            }
            else
            {
                validEndTime = true;
            }
        }

        try
        {
            var res = await _shiftApiService.CreateShiftAsync(new ShiftCreateDTO { WorkerId = worker.Id, ShiftStart = startTime, ShiftEnd = endTime });
            if (res != null)
            {
                Console.WriteLine($"\nShift created successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private async Task UpdateShiftTimeInput(ShiftReadDTO shift)
    {
        Console.WriteLine("\nUPDATE SHIFT TIME");

        string format = "dd/MM/yyyy HH:mm:ss";

        Console.WriteLine($"Please choose new starting time with exact format ({format})");
        Console.Write("Your choice:\t");
        string? inp = Console.ReadLine();

        bool isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime);
        while (!isSuccess)
        {
            Console.WriteLine($"Please enter a valid time with format ({format}) or 0 to exit");
            Console.Write("Try again:\t");
            inp = Console.ReadLine();

            if (inp == "0")
            {
                return;
            }

            isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime);
        }

        bool validEndTime = false;
        DateTime endTime = DateTime.MinValue;

        while (!validEndTime)
        {
            Console.WriteLine($"Please choose new ending time with exact format ({format})");
            Console.Write("Your choice:\t");
            inp = Console.ReadLine();

            isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime);
            while (!isSuccess)
            {
                Console.WriteLine($"Please enter a valid time with format ({format}) or 0 to exit");
                Console.Write("Try again:\t");
                inp = Console.ReadLine();

                if (inp == "0")
                {
                    return;
                }

                isSuccess = DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime);
            }

            if (endTime <= startTime)
            {
                Console.WriteLine("\nError: End time must be after start time! Please try again.");
            }
            else
            {
                validEndTime = true;
            }
        }

        try
        {
            var res = await _shiftApiService.UpdateShiftAsync(shift.Id, new ShiftCreateDTO { WorkerId = shift.WorkerId, ShiftStart = startTime, ShiftEnd = endTime });
            if (res)
            {
                Console.WriteLine($"\nShift updated successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private async Task UpdateShiftWorkerInput(ShiftReadDTO shift)
    {
        Console.WriteLine("\nUPDATE SHIFT WORKER");

        var worker = await ChooseWorkerInput();
        if (worker == null)
        {
            return;
        }

        try
        {
            var res = await _shiftApiService.UpdateShiftAsync(shift.Id, new ShiftCreateDTO { WorkerId = worker.Id, ShiftStart = shift.ShiftStart, ShiftEnd = shift.ShiftEnd });
            if (res)
            {
                Console.WriteLine($"\nShift updated successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private async Task DeleteShiftInput(ShiftReadDTO shift)
    {
        try
        {
            var res = await _shiftApiService.DeleteShiftAsync(shift.Id);
            if (res)
            {
                Console.WriteLine($"\nShift deleted successfully!");
            }
            else
            {
                Console.WriteLine($"\nError occurred while deleting shift!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return;
        }
    }

    private ShiftReadDTO? ChooseShiftInput(List<ShiftReadDTO> shifts)
    {
        if (shifts == null || shifts.Count == 0)
        {
            return null;
        }

        string? inp;
        int shiftId = -1;

        Console.WriteLine("\nChoose a shift by ID or go back to menu by typing 0:\t");

        while (true)
        {
            Console.Write("Your choice:\t");
            inp = Console.ReadLine()?.Trim() ?? "";

            if (inp == "0")
            {
                return null;
            }

            if (Validator.IsValidString(inp) && Int32.TryParse(inp, out shiftId) && shifts.Any(x => x.Id == shiftId))
            {
                break;
            }

            Console.WriteLine("\nInvalid shift ID. Please choose an ID from the list above or 0 to exit.");
        }

        return shifts.FirstOrDefault(x => x.Id == shiftId);
    }

    private async Task<WorkerReadDTO?> ChooseWorkerInput()
    {
        List<WorkerReadDTO>? workers;
        try
        {
            workers = await _workerApiService.GetAllWorkersAsync();

            if (workers == null || workers.Count == 0)
            {
                Console.WriteLine("\nNo worker found!");
                return null;
            }

            TableVisualizer.ShowTable<WorkerReadDTO>(workers, "WORKERS LIST");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected error: {ex.Message}");
            return null;
        }

        return InputHelper.ChooseWorker(workers);
    }
}
