namespace ShiftsLogger.UI.DTOs;

public class ShiftDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public override string ToString()
    {
        return $"{StartTime} | {EndTime}";
    }
}