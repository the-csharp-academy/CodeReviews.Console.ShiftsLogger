namespace ShiftLoggerAPI.Contracts.Shifts;

public sealed record ShiftResponse(int Id, DateTime Start, DateTime End, TimeSpan Duration);