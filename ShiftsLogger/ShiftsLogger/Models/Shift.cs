namespace ShiftsLogger.Models;

public class Shift
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public TimeSpan Duration => ShiftEnd - ShiftStart;
    public Worker? Worker { get; set; }
}
