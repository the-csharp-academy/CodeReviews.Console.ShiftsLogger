using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Model;

public class Shift
{
    [Key]
    public int ShiftId { get; set; }
    public required string WorkerName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal HoursWorked { get; set; }
    public required string ShiftTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
}