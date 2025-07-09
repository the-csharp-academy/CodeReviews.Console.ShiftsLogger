using System.ComponentModel.DataAnnotations;

namespace Shifts_Logger_UI.Enums;

public class Enums
{
    public enum MenuOptions
    {
        [Display(Name = "View All Shifts")]
        ViewAllShifts,

        [Display(Name = "View Shift by ID")]
        ViewShiftById,

        [Display(Name = "Create Shift")]
        CreateShift,

        [Display(Name = "Update Shift")]
        UpdateShift,

        [Display(Name = "Delete Shift")]
        DeleteShift,

        [Display(Name = "Exit")]
        Exit,
    }
}
