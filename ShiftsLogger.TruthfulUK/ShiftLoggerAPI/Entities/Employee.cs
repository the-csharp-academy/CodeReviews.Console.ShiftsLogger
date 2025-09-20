namespace ShiftLoggerAPI.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public Employee(string name, string title)
    {
        Name = name;
        Title = title;
    }

    public void Update(string name, string title)
    {
        if (name != string.Empty)
        {
            Name = name;
        }

        if (title != string.Empty)
        {
            Title = title;
        }
    }

}
