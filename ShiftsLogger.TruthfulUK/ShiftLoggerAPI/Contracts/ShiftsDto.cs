namespace ShiftLoggerAPI.Contracts;

public sealed record CreateShiftRequest(DateTime Start, DateTime End, int EmployeeId);
public sealed record UpdateShiftRequest(DateTime Start, DateTime End, int EmployeeId);
public sealed record ShiftResponse(int Id, DateTime Start, DateTime End, TimeSpan Duration);