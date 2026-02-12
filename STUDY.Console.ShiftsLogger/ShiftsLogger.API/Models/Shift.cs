using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.API.Models;

public class Shift
{
    public int Id { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public int? WorkerId { get; set; }
    public Worker? Worker { get; set; }
}
