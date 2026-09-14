namespace ShiftsLogger.UI.Models;

/// <summary>Body sent to the API for create and update.</summary>
public class ShiftRequest
{
    public string WorkerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
