using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.DTOs;

public class CreatedWorkerShiftDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string WorkerName { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    [Required]
    [RegularExpression("Morning|Afternoon|Night")]
    public string ShiftTime { get; set; } = string.Empty;
}