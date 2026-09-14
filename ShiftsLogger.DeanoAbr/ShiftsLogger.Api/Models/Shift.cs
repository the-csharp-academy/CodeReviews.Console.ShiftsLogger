using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftsLogger.Api.Models;

/// <summary>A single work shift for one worker.</summary>
public class Shift
{
    public int Id { get; set; }

    public string WorkerName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    /// <summary>Computed on read; never stored (see [NotMapped]).</summary>
    [NotMapped]
    public TimeSpan Duration => EndTime - StartTime;
}
