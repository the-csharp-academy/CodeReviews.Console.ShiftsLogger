using ShiftsLogger.ConsoleApp.Models;
using ShiftsLogger.ConsoleApp.Services;
using Spectre.Console;

namespace ShiftsLogger.ConsoleApp.Views;

public class UserInterface
{

    private readonly ShiftService _shiftService;

    private readonly WorkerService _workerService;

    public UserInterface(ShiftService shiftService, WorkerService workerService)
    {
        _shiftService = shiftService;
        _workerService = workerService;
    }





    public string ReadString(string question)
    {
        Console.Write(question);
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Invalid input\n");
            answer = ReadString(question);
        }

        return answer;
    }


    public string ReadString(string question, string[] options)
    {
        Console.WriteLine(question);
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            Console.WriteLine("Invalid input\n");
            answer = ReadString(question, options);
        }

        if (!options.Contains(answer.ToLower()))
        {
            Console.WriteLine("Invalid option\n");
            answer = ReadString(question, options);
        }

        return answer;
    }

    public string ReadString(string question, string currentValue)
    {
        AnsiConsole.Ask<string>(question);
        Console.WriteLine("New Value: ");
        var answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || string.IsNullOrWhiteSpace(answer))
        {
            answer = currentValue;
        }

        return answer;
    }

    public TimeOnly PromptForTime(string promptText)
    {
        TimeOnly? time;
        do
        {
            Console.Write($"\nEnter the {promptText} time (hh:mm am/pm): ");
            string? timeInput = Console.ReadLine();
            time = Validation.ValidateTime(timeInput);
            if (time == null)
            {
                Console.WriteLine("Invalid time format. Please enter a time in the format hh:mm am/pm.");
            }
        } while (time == null);

        return time.Value;
    }

    public DateOnly PromptForDate()
    {
        DateOnly? date;
        do
        {
            Console.Write("\nEnter the date (mm/dd/yyyy): ");
            string? dateInput = Console.ReadLine();
            date = Validation.ValidateDate(dateInput!);
            if (date == null)
            {
                Console.WriteLine("Invalid date format. Please enter a date in the format mm/dd/yyyy.");
            }
        } while (date == null);

        return date.Value;
    }


    public WorkerDto SelectWorker(string title, List<WorkerDto> choices)
    {
        if (choices.Count == 0)
        {
            Console.WriteLine("\nNo workers exist.");
            return new WorkerDto();
        }

        var formattedChoices = choices.Select(c => $"{c.FirstName} {c.LastName}, {c.Title}").ToList();

        var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .AddChoices(formattedChoices));

        return choices.First(c => $"{c.FirstName} {c.LastName}, {c.Title}" == option);
    }

    public ShiftDto SelectShift(string title, List<ShiftDto> choices)
    {
        if (choices.Count == 0)
        {
            Console.WriteLine("\nNo shifts exist.");
            return new ShiftDto();
        }

        var sortedChoices = choices
        .OrderByDescending(c => c.Date)      // Primary sort: Date descending
        .ThenByDescending(c => c.StartTime)  // Secondary sort: StartTime descending
        .ToList();

        var formattedChoices = sortedChoices.Select(c => $"{c.Date, -12} {c.ShiftName, -5} {c.StartTime, 10} {c.EndTime, 10} {c.Duration.ToString("H:mm"), 8}     {c.Worker.FirstName, -10} {c.Worker.LastName, -15} {c.Worker.Title, -10}").ToList();

        var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .AddChoices(formattedChoices));

        return choices.First(c => $"{c.Date,-12} {c.ShiftName,-5} {c.StartTime,10} {c.EndTime,10} {c.Duration.ToString("H:mm"),8}     {c.Worker.FirstName,-10} {c.Worker.LastName,-15} {c.Worker.Title,-10}" == option);
    }



    public async Task<ShiftDto> PromptForNewShift()
    {
        Console.Clear();
        var shifts = await _shiftService.GetShiftsAsync();
        Display.PrintShiftsTable(shifts, "Add Shift");

        ShiftDto shiftDto = new ShiftDto();



        return shiftDto;
    }
}