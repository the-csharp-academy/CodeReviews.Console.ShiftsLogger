using Spectre.Console;

public class AppView : IAppView
{
    public void DisplayGoodbye()
        => AnsiConsole.MarkupLine("[green]Goodbye![/]");

    public MainMenuOption DisplayMainMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOption>()
                .Title("[green]Shifts Logger[/]")
                .AddChoices(Enum.GetValues<MainMenuOption>())
                .UseConverter(FormatMenuOption));
    }

    public PortalMenuOption DisplayPortalMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<PortalMenuOption>()
                .Title("[green]Enter portal[/]")
                .AddChoices(Enum.GetValues<PortalMenuOption>())
                .UseConverter(option => option switch
                {
                    PortalMenuOption.Shifts => "Shifts",
                    PortalMenuOption.Employees => "Employees",
                    PortalMenuOption.Back => "Back",
                    _ => option.ToString()
                }));
    }

    public void DisplayMessage(string message)
        => AnsiConsole.MarkupLine(Markup.Escape(message));

    public void WaitForInput()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue.[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    private static string FormatMenuOption(MainMenuOption option)
    {
        return option switch
        {
            MainMenuOption.EnterPortal => "Enter portal",
            MainMenuOption.Exit => "Exit",
            _ => option.ToString()
        };
    }

}