namespace ShiftsLogger.UI.DTOs;

public class ShiftCreateDTO
{
    public int WorkerId { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
}
