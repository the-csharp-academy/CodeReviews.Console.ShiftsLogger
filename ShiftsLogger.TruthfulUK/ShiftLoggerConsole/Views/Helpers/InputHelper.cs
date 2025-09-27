using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace ShiftLoggerConsole.Views.Helpers;
internal static class InputHelper
{
    public static Dictionary<string, TEnum> GetMenuOptions<TEnum>()
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .ToDictionary(option => GetEnumDisplayName(option), option => option);
    }

    private static string GetEnumDisplayName(Enum value)
    {
        return value.GetType()
            .GetMember(value.ToString())[0]
            .GetCustomAttribute<DisplayAttribute>()?.Name
            ?? value.ToString();
    }

    public static TEnum SelectionPrompt<TEnum>(Dictionary<string, TEnum> options)
        where TEnum : struct, Enum
    {
        var selectedKey = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Please select an [chartreuse1]option[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(options.Keys));

        return options[selectedKey];
    }

    public static DateOnly DatePrompt()
    {
        var dateString = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter date (DD/MM/YY):")
            .DefaultValue(DateTime.Now.ToString("dd/MM/yy"))
            .DefaultValueStyle(Color.Chartreuse1)
            .Validate(input =>
            {
                return DateTime.TryParseExact(input, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                ? Spectre.Console.ValidationResult.Success()
                : Spectre.Console.ValidationResult.Error("\n[red]Invalid date, please use DD/MM/YY[/]\n");
            })
        );
        return DateOnly.ParseExact(dateString, "dd/MM/yy", CultureInfo.InvariantCulture);
    }

    public static TimeOnly TimePrompt()
    {
        var timeString = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter time (24 hour format - HH:MM):")
            .DefaultValue(DateTime.Now.ToString("HH:mm"))
            .DefaultValueStyle(Color.Chartreuse1)
            .Validate(input =>
            {
                return TimeOnly.TryParseExact(input, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                ? Spectre.Console.ValidationResult.Success()
                : Spectre.Console.ValidationResult.Error("\n[red]Invalid time, please use (HH:MM) and enter a valid 24 hour value (e.g., 23:25 for 11:25PM)[/]\n");
            })
        );
        return TimeOnly.ParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture);
    }

    public static DateTime GetDateTime()
    {
        var date = DatePrompt();
        var time = TimePrompt();
        return date.ToDateTime(time);
    }

    public static (DateTime Start, DateTime End) GetValidatedShiftSpan(
        string startLabel = "Enter Shift Start",
        string endLabel = "Enter Shift End")
    {
        AnsiConsole.MarkupLine($"[chartreuse1]{startLabel}[/]");
        var start = GetDateTime();

        while (true)
        {
            AnsiConsole.MarkupLine($"\n[chartreuse1]{endLabel}[/]");
            var end = GetDateTime();

            if (end > start)
                return (start, end);

            AnsiConsole.MarkupLine("\n[red]End must be after the start. Try again.[/]\n");
        }
    }

    public static void Header(string title)
    {
        AnsiConsole.Clear();

        AnsiConsole.MarkupLine("[bold]Shift Logger Console Client[/] [gray]| version 1.0[/]");

        var rule =
            new Rule($"{title}")
                .RuleStyle("grey dim")
                .Centered();

        AnsiConsole.Write(rule);
    }

    public static void Pause()
    {
        var rule = new Rule().RuleStyle("grey dim");
        AnsiConsole.Write(rule);

        var paddedContinueText =
            new Text("Display paused - press any key to continue",
            new Style(Color.Grey23));
        var paddedContinue = new Padder(paddedContinueText).PadTop(1);
        AnsiConsole.Write(paddedContinue);
        Console.ReadKey();
    }
}
