using Spectre.Console;
using static silvermax.shiftlogger.UI.Enums;

namespace silvermax.shiftlogger.UI;

internal class UserInterface
{
    private readonly ShiftManager _manager;

    public UserInterface(ShiftManager manager) => _manager = manager;

    public async Task Start()
    {
        bool openManager = true;

        while (openManager)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("Welcome to the Shift Manager.")
                .UseConverter(e => Enums.DisplayChoice(e))
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (choice)
            {
                case MenuOptions.AddShift:
                    await _manager.CreateNewShiftInteractive();
                    break;

                case MenuOptions.UpdateShift:
                    await _manager.UpdateShiftInteractive();
                    break;

                case MenuOptions.SeeAllShifts:
                    await _manager.DisplayAllShiftsInteractive();
                    break;

                case MenuOptions.DeleteShift:
                    await _manager.DeleteshiftInteractive();
                    break;

                case MenuOptions.Exit:
                    openManager = false;
                    break;
            }
        }
    }

}