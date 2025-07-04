using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using ShiftsLogger.ConsoleApp.Views;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Controllers;

internal class ShiftController
{
    public static async Task ViewShifts(string heading, int? shiftId = null)
    {
        var shiftService = new ShiftService(new HttpClient());
        List<ShiftDto> viewShifts = new List<ShiftDto>();

        if (shiftId.HasValue)
        {
            try
            {
                var shift = await shiftService.GetShiftAsync(shiftId.Value);
                viewShifts = shift is not null ? new List<ShiftDto> { shift } : new List<ShiftDto>();
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                Console.WriteLine($"\nFailed to retrieve shift with ID {shiftId.Value}. Request error: {e.Message}");
                return; // Exit early if shift retrieval fails
            }
        }
        else
        {
            try
            {
                viewShifts = await shiftService.GetShiftsAsync();
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                Console.WriteLine("\nFailed to retrieve shifts. Request error: " + e.Message);
                return; // Exit early if shifts retrieval fails
            }
        }

        Display.PrintShiftsTable(viewShifts, heading);
    }

    public static async Task CreateShift()
    {
        try
        {
            await ViewShifts("Add Shift");
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve shifts before creating a new one. Request error: {e.Message}");
            return; // Exit early if shifts can't be retrieved
        }
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        var newShiftName = userInterface.ReadString("\nEnter Shift (1st, 2nd, or 3rd): ");
        var newDate = userInterface.PromptForDate();
        var newStartTime = userInterface.PromptForTime("start");
        var newEndTime = userInterface.PromptForTime("end");
        while (Validation.ValidateStartTimeIsLessThanEndTime(newStartTime, newEndTime) == false)
        {
            Console.WriteLine("The end time must be later than the start time.");
            newEndTime = userInterface.PromptForTime("end");
        }

        var workerService = new WorkerService(new HttpClient());

        List<WorkerDto> shiftWorkers = new List<WorkerDto>();

        try
        {
            shiftWorkers = await workerService.GetWorkersAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve workers. Request error: {e.Message}");
            return; // Exit early if workers can't be retrieved
        }

        var newShiftWorker = userInterface.SelectWorker("\nChoose Worker for Shift:", shiftWorkers);

        var shift = new ShiftDto { Date = newDate, ShiftName = newShiftName, StartTime = newStartTime, EndTime = newEndTime, WorkerId = newShiftWorker.WorkerId, Worker = newShiftWorker };
        try
        {
            var insertResult = await shiftService.InsertShiftAsync(shift);
            Console.Clear();

            try
            {
                await ViewShifts("Add Shift");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nFailed to refresh shift list after creation. Request error: {e.Message}");
            }

            if (insertResult)
            {
                Console.WriteLine("\nSuccessfully added shift!");
            }
            else
            {
                Console.WriteLine("\nFailed to add shift.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            try
            {
                await ViewShifts("Add Shift");
            }
            catch (HttpRequestException viewError)
            {
                Console.WriteLine($"\nFailed to refresh shift list after failed creation. Request error: {viewError.Message}");
            }

            Console.WriteLine($"\nFailed to add shift. Request error: {e.Message}");
        }
    }

    public static async Task EditShift()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        List<ShiftDto> editShifts = new List<ShiftDto>();

        try
        {
            editShifts = await shiftService.GetShiftsAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve shifts. Request error: {e.Message}");
            return; // Exit early if shifts can't be retrieved
        }
        var rule = new Rule("[green]Edit Shift[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var editShift = userInterface.SelectShift("\nChoose Shift to Edit:", editShifts);
        if (editShift.ShiftName == null)
        {
            return;
        }

        try
        {
            await ViewShifts("Edit Shift", editShift.ShiftId);
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve shift details. Request error: {e.Message}");
            return; // Exit early if shift details can't be retrieved
        }
        Console.WriteLine();
        editShift.Date = AnsiConsole.Confirm("Update shift's date?", false) ? AnsiConsole.Ask<DateOnly>("Shift's new date:") : editShift.Date;
        Console.WriteLine();
        editShift.ShiftName = AnsiConsole.Confirm("Update shift name?", false) ? AnsiConsole.Ask<string>("Shifts's new  name:") : editShift.ShiftName;
        Console.WriteLine();
        editShift.StartTime = AnsiConsole.Confirm("Update shift's start time?", false) ? AnsiConsole.Ask<TimeOnly>("Shift's new start time:") : editShift.StartTime;
        Console.WriteLine();
        editShift.EndTime = AnsiConsole.Confirm("Update shift's end time?", false) ? AnsiConsole.Ask<TimeOnly>("Shift's new end time:") : editShift.EndTime;
        Console.WriteLine();
        var workerService = new WorkerService(new HttpClient());
        List<WorkerDto> shiftWorkers = new List<WorkerDto>();

        try
        {
            shiftWorkers = await workerService.GetWorkersAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve workers. Request error: {e.Message}");
            return; // Exit early if workers can't be retrieved
        }
        editShift.Worker = AnsiConsole.Confirm("Update shift's worker?", false) ? userInterface.SelectWorker("\nChoose Worker for Shift:", shiftWorkers) : editShift.Worker;
        editShift.WorkerId = editShift.Worker.WorkerId;

        try
        {
            var editResult = await shiftService.UpdateShift(editShift);
            Console.Clear();
            await ViewShifts("Edit Shift");

            if (editResult)
            {
                Console.WriteLine("\nSuccessfully edited shift!");
            }
            else
            {
                Console.WriteLine("\nFailed to edit shift.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            await ViewShifts("Edit Shift");
            Console.WriteLine($"\nFailed to edit shift. Request error: {e.Message}");
        }
    }

    public static async Task DeleteShift()
    {
        var userInterface = new UserInterface(new ShiftService(new HttpClient()), new WorkerService(new HttpClient()));
        var shiftService = new ShiftService(new HttpClient());
        List<ShiftDto> deleteShifts = new List<ShiftDto>();

        try
        {
            deleteShifts = await shiftService.GetShiftsAsync();
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve shifts. Request error: {e.Message}");
            return; // Exit early if shifts can't be retrieved
        }
        var rule = new Rule("[green]Delete Shift[/]");
        rule.Justification = Justify.Left;
        AnsiConsole.Write(rule);

        var deleteShift = userInterface.SelectShift("\nChoose Shift to Delete:", deleteShifts);
        if (deleteShift.ShiftName == null)
        {
            return;
        }

        try
        {
            await ViewShifts("Delete Shift", deleteShift.ShiftId);
        }
        catch (HttpRequestException e)
        {
            Console.Clear();
            Console.WriteLine($"\nFailed to retrieve shift details. Request error: {e.Message}");
            return; // Exit early if shift details can't be retrieved
        }

        Console.WriteLine();

        if (AnsiConsole.Confirm("[yellow]Do you really want to delete this shift?[/]", false))
        {
            try
            {
                var deleteResult = await shiftService.DeleteShift(deleteShift.ShiftId);
                Console.Clear();
                await ViewShifts("Delete Shift");

                if (deleteResult)
                {
                    Console.WriteLine("\nSuccessfully deleted shift!");
                }
                else
                {
                    Console.WriteLine("\nFailed to delete shift.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.Clear();
                await ViewShifts("Delete Shift");
                Console.WriteLine($"\nFailed to delete shift. Request error: {e.Message}");
            }
        }
        else
        {
            Console.WriteLine("\nShift not deleted.");
        }
    }
}
