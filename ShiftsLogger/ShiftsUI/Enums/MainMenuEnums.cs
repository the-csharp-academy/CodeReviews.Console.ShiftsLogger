using System.ComponentModel;

namespace ShiftsUI.Enums;

internal static class MainMenuEnums
{
    internal enum MainMenuOption
    {
        [Description("Create Shift")]
        Create,       
        [Description("View Shifts")]
        View,        
        [Description("Edit Shift")]
        Update,       
        [Description("Delete Shift")]
        Delete,
        Exit
    }
}