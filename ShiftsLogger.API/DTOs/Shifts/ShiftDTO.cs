public class ShiftDTO
{
    public int Id { get; set; }
    public EmployeeDTO Employee { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
}