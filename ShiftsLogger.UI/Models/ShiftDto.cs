namespace ShiftsLogger.UI.Models;

/// <summary>A shift as returned by the API.</summary>
public record ShiftDto(int Id, string WorkerName, DateTime StartTime, DateTime EndTime);
