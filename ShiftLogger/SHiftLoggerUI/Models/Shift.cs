namespace ShiftLoggerUI;

public class Shift
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int WorkerId { get; set; }

    public string Duration()
    {
        TimeSpan duration = EndTime - StartTime;
        return $"{(int)duration.TotalHours}h {duration.Minutes}m";
    }
}