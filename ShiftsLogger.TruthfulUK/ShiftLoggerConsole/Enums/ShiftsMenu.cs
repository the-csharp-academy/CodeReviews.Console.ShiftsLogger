using System.ComponentModel.DataAnnotations;

namespace ShiftLoggerConsole.Enums;
internal enum ShiftsMenu
{
    [Display(Name = "Log a Shift")]
    LogShift,

    [Display(Name = "View Recent Shifts")]
    ViewRecent,

    [Display(Name = "View Shifts by Employee")]
    ViewEmployeeShifts,

    [Display(Name = "Update a Shift")]
    UpdateShift,

    [Display(Name = "Delete a Shift")]
    DeleteShift,

    [Display(Name = "Back to Main Menu")]
    BackToMainMenu
}
