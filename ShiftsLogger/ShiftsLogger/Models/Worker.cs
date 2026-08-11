namespace ShiftsLogger.Models;

public class Worker
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
