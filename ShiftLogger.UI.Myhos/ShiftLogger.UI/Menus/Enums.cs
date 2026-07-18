namespace ShiftLogger.UI.Menus;

internal static class Enums
{
    internal enum MainMenuOptions 
    {
        CreateWorkerShift,
        ViewWorkerShift,
        ViewAllWorkerShift,
        UpdateWorkerShift,
        DeleteWorkerShift,
        Exit
    }

    internal enum ShiftOptions 
    {
        Morning,
        Afternoon,
        Night
    }

    public enum UpdateOptions
    {
        WorkerName,
        Date,
        StartTime,
        EndTime,
        ShiftTime,
        SaveChanges,
        Cancel
    }
}
