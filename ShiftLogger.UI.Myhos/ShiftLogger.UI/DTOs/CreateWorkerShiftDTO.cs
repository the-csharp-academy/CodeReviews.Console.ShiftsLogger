namespace ShiftLogger.UI.DTOs;

internal class CreateWorkerShiftDTO
{
    public string WorkerName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ShiftTime { get; set; } = string.Empty;
}