using System.ComponentModel.DataAnnotations;

namespace ShiftLoggerConsole.Enums;
internal enum EmployeesMenu
{
    [Display(Name = "View Employees")]
    ViewEmployees,

    [Display(Name = "Add an Employee")]
    AddEmployee,

    [Display(Name = "Update an Employee")]
    UpdateEmployee,

    [Display(Name = "Delete an Employee")]
    DeleteEmployee,

    [Display(Name = "Back to Main Menu")]
    BackToMainMenu
}