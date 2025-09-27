using System.ComponentModel.DataAnnotations;

namespace ShiftLoggerConsole.Enums;
internal enum MainMenu
{
    [Display(Name = "Shifts")]
    Shifts,

    [Display(Name = "Employees")]
    Employees,

    [Display(Name = "Exit Application")]
    ExitApplication
}
