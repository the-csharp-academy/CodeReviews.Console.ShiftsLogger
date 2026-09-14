namespace ShiftsLogger.Api.Dtos;

/// <summary>What the API returns for a shift.</summary>
public record ShiftDto(int Id, string WorkerName, DateTime StartTime, DateTime EndTime);
