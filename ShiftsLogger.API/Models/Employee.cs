public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool isActive { get; set; } = true;
    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}