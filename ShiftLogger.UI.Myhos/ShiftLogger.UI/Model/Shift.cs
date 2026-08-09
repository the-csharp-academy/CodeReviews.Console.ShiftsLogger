namespace ShiftLogger.UI.Model;

internal class Shift
{
    public int ShiftId { get; set; }
    public string WorkerName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal HoursWorked { get; set; }
    public string ShiftTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
}