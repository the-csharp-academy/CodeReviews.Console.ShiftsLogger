namespace ShiftsLogger.UI.DTOs;

public class ShiftCreationDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int WorkerId { get; set; }
}