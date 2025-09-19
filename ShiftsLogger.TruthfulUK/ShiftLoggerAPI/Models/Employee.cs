namespace ShiftLoggerAPI.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public IList<Shift> Shifts { get; set; }
    public Employee()
    {
        Shifts = new List<Shift>();
    }
}
