using System.ComponentModel.DataAnnotations;

namespace ShiftLoggerConsole.Enums;
internal enum EmployeesMenu
{
    [Display(Name = "View Employees")]
    LogShift,

    [Display(Name = "Add an Employee")]
    ViewRecent,

    [Display(Name = "Update an Employee")]
    ViewEmployeeShifts,

    [Display(Name = "Delete an Employee")]
    UpdateShift,

    [Display(Name = "Back to Main Menu")]
    BackToMainMenu
}