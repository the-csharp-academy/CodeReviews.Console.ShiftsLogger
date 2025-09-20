namespace ShiftLoggerAPI.Entities;

public class Shift
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => End - Start;
    public int EmployeeId { get; set; }

    public Shift(DateTime start, DateTime end, int employeeId)
    {
        Start = start;
        End = end;
        EmployeeId = employeeId;
    }

    public void Update(DateTime start, DateTime end, int employeeId)
    {
        if (Start != start) Start = start;
        if (End != end) End = end;
        if (EmployeeId != employeeId) EmployeeId = employeeId;
    }
}
