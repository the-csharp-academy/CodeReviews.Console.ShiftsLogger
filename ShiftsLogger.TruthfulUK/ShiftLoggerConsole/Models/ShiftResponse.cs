namespace ShiftLoggerConsole.Models;
internal class ShiftResponse
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get; set; }
    public int EmployeeId { get; set; }
}
