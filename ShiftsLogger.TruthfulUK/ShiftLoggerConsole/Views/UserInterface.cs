using ShiftLoggerConsole.Controllers;
using ShiftLoggerConsole.Enums;
using ShiftLoggerConsole.Views.Helpers;

namespace ShiftLoggerConsole.Views;
internal class UserInterface
{
    private ShiftController _shifts = new ShiftController();

    public async Task Display()
    {
        while (true)
        {
            InputHelper.Header("Welcome");

            var menuOptions = InputHelper.GetMenuOptions<MainMenu>();
            var menuChoice = InputHelper.SelectionPrompt(menuOptions);

            switch (menuChoice)
            {
                case MainMenu.Shifts:
                    var shiftMenuOptions = InputHelper.GetMenuOptions<ShiftsMenu>();
                    var shiftMenuChoice = InputHelper.SelectionPrompt(shiftMenuOptions);
                    switch (shiftMenuChoice)
                    {
                        case ShiftsMenu.LogShift:
                            await _shifts.LogShift();
                            break;
                        case ShiftsMenu.ViewRecent:
                            await _shifts.GetAllShifts();
                            InputHelper.Pause();
                            break;
                    }
                    break;
                case MainMenu.Employees:
                    var employeeMenuOptions = InputHelper.GetMenuOptions<EmployeesMenu>();
                    var employeeMenuChoice = InputHelper.SelectionPrompt(employeeMenuOptions);
                    break;
                case MainMenu.ExitApplication:
                    Environment.Exit(0);
                    break;
            }

        }
    }
}
