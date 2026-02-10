namespace ShiftsLogger.UI.DTOs;

public class WorkerDetailsDTO
{
    public string Name { get; set; } = null!;
    public IReadOnlyList<ShiftDto> Shifts { get; set; } = [];
}