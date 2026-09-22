using ShiftLogger.Domain.Validation.Errors;
using Spectre.Console;

namespace ShiftLogger.Console.Presentation.Output;

internal static class Messages
{
    public static void OutputErrorMessage(Error error)
    {
        var errors = new Error[] { error };
        OutputErrorMessage(errors);
    }
    public static void OutputErrorMessage(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            AnsiConsole.MarkupLine($"[red]ERROR:[/] {error.Code} -- {error.Description}");
        }
    }
    public static void Success(string message)
    {
        AnsiConsole.MarkupLine($"[green]SUCCESS:[/] {message}!");
    }
    public static void Cancelled(string message)
    {
        AnsiConsole.MarkupLine($"[red]CANCELLED:[/] {message}!");
    }
    public static void GoodbyeMessage()
    {
        AnsiConsole.WriteLine("Goodbye!");
        PressAnyKeyToContinue();
    }
    public static void PressAnyKeyToContinue()
    {
        AnsiConsole.WriteLine("Press any key to continue");
        AnsiConsole.Console.Input.ReadKey(false);
    }
    public static void PrintBlankLines(int lines)
    {
        for (int i = 0; i < lines; i++)
        {
            AnsiConsole.WriteLine();
        }
    }
    public static string GenerateDurationString(DateTime startTime, DateTime endTime)
    {
        var duration = TimeSpan.FromSeconds((endTime - startTime).TotalSeconds);
        return $"{(int)Math.Floor(duration.TotalHours):D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
    }
}