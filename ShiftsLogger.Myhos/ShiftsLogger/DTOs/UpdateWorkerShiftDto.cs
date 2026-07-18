namespace ShiftsLogger.DTOs;

public class UpdateWorkerShiftDto
{
    public string? WorkerName { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public decimal? HoursWorked { get; set; }
    public string? ShiftTime { get; set; }
}