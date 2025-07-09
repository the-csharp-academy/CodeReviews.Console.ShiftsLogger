using System.ComponentModel.DataAnnotations;

namespace Shifts_Logger.Models;

public class Shift
{
    [Key]
    public int Id { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public int ShiftNumber { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan Duration => EndTime - StartTime;
}
