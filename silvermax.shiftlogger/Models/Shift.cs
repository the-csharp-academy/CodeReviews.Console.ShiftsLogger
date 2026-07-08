using System.ComponentModel.DataAnnotations.Schema;

namespace silvermax.shiftlogger.Models;

public class Shift
{
    public Guid ShiftId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public TimeSpan Duration { get; set; }
}
