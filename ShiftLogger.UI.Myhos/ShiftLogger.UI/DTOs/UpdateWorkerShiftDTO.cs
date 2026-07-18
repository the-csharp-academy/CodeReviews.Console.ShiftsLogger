namespace ShiftLogger.UI.DTOs;

internal class UpdateWorkerShiftDTO
{
    public string? WorkerName { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? ShiftTime { get; set; }
}