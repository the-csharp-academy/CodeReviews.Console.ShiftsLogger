namespace silvermax.shiftlogger.Dtos;

public class CreateShiftDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
