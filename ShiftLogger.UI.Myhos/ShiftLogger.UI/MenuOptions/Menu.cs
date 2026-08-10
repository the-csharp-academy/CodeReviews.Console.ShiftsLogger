using ShiftLogger.UI.Enums;
using ShiftLogger.UI.Extensions;
using ShiftLogger.UI.MenuOptions;
using Spectre.Console;

namespace ShiftLogger.UI.Menus;

internal class Menu
{
    private readonly ShiftManager shiftManager = new();
    internal async Task MainMenu()
    {
        bool open = true;

        while (open)
        {
            Console.Clear();

            var options = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                .Title("Select an option: ")
                .UseConverter(option => option.ToDisplayString())
                .AddChoices(Enum.GetValues<MainMenuOptions>()));

            switch (options)
            {
                case MainMenuOptions.CreateShift:
                    await shiftManager.CreateWorkerShift();
                    break;
                case MainMenuOptions.ViewAllShift:
                    await shiftManager.ViewAll();
                    break;
                case MainMenuOptions.ViewShift:
                    await shiftManager.ViewWorkerShiftById();
                    break;
                case MainMenuOptions.UpdateShift:
                    await shiftManager.UpdateWorkerShift();
                    break;
                case MainMenuOptions.DeleteShift:
                    await shiftManager.DeleteWorkerShift();
                    break;
                case MainMenuOptions.Exit:
                    open = false;
                    break;
            }
        }
    }
}