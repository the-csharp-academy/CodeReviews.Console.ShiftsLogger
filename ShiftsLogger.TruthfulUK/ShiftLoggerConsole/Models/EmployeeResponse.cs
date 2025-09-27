namespace ShiftLoggerConsole.Models;
internal class EmployeeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public override string ToString() => Name;
}
