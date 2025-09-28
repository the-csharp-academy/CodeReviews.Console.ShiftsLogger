using ShiftLoggerConsole.Handlers;
using ShiftLoggerConsole.Enums;
using ShiftLoggerConsole.Views.Utilities;

namespace ShiftLoggerConsole.Views;
internal class UserInterface
{
    private ShiftHandler _shifts = new ShiftHandler();
    private EmployeeHandler _employees = new EmployeeHandler();

    public async Task Display()
    {
        while (true)
        {
            Helpers.Header("Welcome");

            var menuOptions = Helpers.GetMenuOptions<MainMenu>();
            var menuChoice = Helpers.SelectionPrompt(menuOptions);

            switch (menuChoice)
            {
                case MainMenu.Shifts:
                    var shiftMenuOptions = Helpers.GetMenuOptions<ShiftsMenu>();
                    var shiftMenuChoice = Helpers.SelectionPrompt(shiftMenuOptions);
                    switch (shiftMenuChoice)
                    {
                        case ShiftsMenu.LogShift:
                            await _shifts.LogShift();
                            break;

                        case ShiftsMenu.ViewShifts:
                            await _shifts.GetAllShifts();
                            break;

                        case ShiftsMenu.ViewEmployeeShifts:
                            await _shifts.GetEmployeeShifts();
                            break;

                        case ShiftsMenu.UpdateShift:
                            await _shifts.UpdateShift();
                            break;

                        case ShiftsMenu.DeleteShift:
                            await _shifts.DeleteShift();
                            break;

                        case ShiftsMenu.BackToMainMenu:
                            break;
                    }
                    break;
                case MainMenu.Employees:
                    var employeeMenuOptions = Helpers.GetMenuOptions<EmployeesMenu>();
                    var employeeMenuChoice = Helpers.SelectionPrompt(employeeMenuOptions);
                    switch (employeeMenuChoice)
                    {
                        case EmployeesMenu.ViewEmployees:
                            await _employees.ViewEmployees();
                            break;
                    
                        case EmployeesMenu.AddEmployee:
                            await _employees.AddEmployee();
                            break;

                        case EmployeesMenu.UpdateEmployee:
                            await _employees.UpdateEmployee();
                            break;

                        case EmployeesMenu.DeleteEmployee:
                            await _employees.DeleteEmployee();
                            break;

                        case EmployeesMenu.BackToMainMenu:
                        break;
                    }
                    break;
                case MainMenu.ExitApplication:
                    Environment.Exit(0);
                    break;
            }

        }
    }
}
