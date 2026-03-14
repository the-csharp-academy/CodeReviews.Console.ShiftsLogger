using ClassLibray.Models;
using Spectre.Console;
using static ShiftsUI.Enums.EnumExtensions;
using static ShiftsUI.Enums.MainMenuEnums;

namespace ShiftsUI.Views;

internal interface IShiftView
{
    MainMenuOption ShowMainMenu();
    string GetInput(string heading, string msg, string error = null!);
    void ReturnToMenu(string msg = null!);
    void ShowShiftList(List<Shift> shifts);
    Shift ShowShiftPrompt(List<Shift> shifts);
}
internal class ShiftView : IShiftView
{
    public MainMenuOption ShowMainMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOption>()
            .Title("[#f6cf71]Main menu[/]")
            .AddChoices(Enum.GetValues<MainMenuOption>())
            .UseConverter(x => $"[#f89c74]{GetDescription(x)}[/]")
            .WrapAround()
            .HighlightStyle("#66c6cc")
            );
        return choice;
    }

    public string GetInput(string heading, string msg, string error = null!)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[#f6cf71]{heading}[/]\n");
        if (error != null) AnsiConsole.MarkupLine($"[red]{error}[/]");
        var input = AnsiConsole.Ask<string>($"[#f89c74]{msg}:[/]");
        return input;
    }

    public void ShowShiftList(List<Shift> shifts)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[#f6cf71]Saved shifts[/]\n");
        AnsiConsole.MarkupLine($"[#f89c74]Id\tStarted\t\t\tEnded\t\t\tDuration[/]");
        foreach (var s in shifts)
        {
            AnsiConsole.MarkupLine($"{s.id}\t{s.StartTime}\t{s.EndTime}\t{s.Duration}");
        }
        ReturnToMenu();
    }

    public Shift ShowShiftPrompt(List<Shift> shifts)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[#f6cf71]Choose a shift[/]\n");
        AnsiConsole.MarkupLine($"[#f89c74]Id\tStarted\t\t\tEnded\t\t\tDuration[/]");
        return AnsiConsole.Prompt(
            new SelectionPrompt<Shift>()
            .AddChoices(shifts)
            .UseConverter(x => $"{x.id}\t{x.StartTime}\t{x.EndTime}\t{x.Duration}")
            .HighlightStyle("#66c6cc")
            .WrapAround()
            );
    }

    public void ReturnToMenu(string msg = null!)
    {
        if (msg != null) AnsiConsole.MarkupLine($"[#f6cf71]{msg}[/]");
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Point)
            .SpinnerStyle("#f89c74")
            .Start("[#f89c74]Press any button to proceed[/]", x =>
            {
                Console.ReadKey();
            });
    }
}