using ShiftLogger.UI.Enums;
using ShiftLogger.UI.Extensions;
using Spectre.Console;

namespace ShiftLogger.UI.MenuOptions;

internal class UserInputs
{
    internal string GetWorkerName()
    {
        string workerName = AnsiConsole.Ask<string>("Worker's name:");

        while (!ValidationInputUser.IsValidWorkerName(workerName))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid Name[/]");
            workerName = AnsiConsole.Ask<string>("Name: ");
        }

        return workerName;
    }

    internal DateOnly GetDateShift()
    {
        string dateString = AnsiConsole.Ask<string>("Date's shift [#44BBA4](yyyy-MM-dd)[/]:");

        DateOnly date;

        while (!ValidationInputUser.IsValidDate(dateString, out date))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid Date[/]");
            dateString = AnsiConsole.Ask<string>("Date: ");
        }

        return date;
    }

    internal TimeOnly GetStartTime()
    {
        string startTime = AnsiConsole.Ask<string>("StarTime [#44BBA4](HH:mm 24-hour format)[/]:");

        TimeOnly time;

        while (!ValidationInputUser.IsValidTime(startTime, out time))
        {
            AnsiConsole.MarkupLine("[#F0362C]Invalid StarTime[/]");
            startTime = AnsiConsole.Ask<string>("Time: ");
        }

        return time;
    }

    internal TimeOnly GetEndTime(TimeOnly startTime)
    {
        while (true)
        {
            string endTimeInput = AnsiConsole.Ask<string>("EndTime [#44BBA4](HH:mm 24-hour format)[/]:");

            if (!ValidationInputUser.IsValidTime(endTimeInput, out TimeOnly endTime))
            {
                AnsiConsole.MarkupLine("[#F0362C]Invalid EndTime[/]");
                continue;
            }

            if (ValidationInputUser.IsStartAndEndTimeEqual(startTime, endTime))
            {
                AnsiConsole.MarkupLine("[#F0362C]StartTime and EndTime cannot be the same[/]");
                continue;
            }

            return endTime;
        }
    }

    internal string GetShiftTime()
    {
        var options = AnsiConsole.Prompt(
            new SelectionPrompt<ShiftOptions>()
            .Title("Select an option")
            .UseConverter(option => option.ToDisplayString())
            .AddChoices(Enum.GetValues<ShiftOptions>()));

        return options.ToString();
    }
}