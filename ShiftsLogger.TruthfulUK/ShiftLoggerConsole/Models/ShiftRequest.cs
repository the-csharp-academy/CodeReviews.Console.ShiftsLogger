namespace ShiftLoggerConsole.Models;
internal class ShiftRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int EmployeeId { get; set; }
}
