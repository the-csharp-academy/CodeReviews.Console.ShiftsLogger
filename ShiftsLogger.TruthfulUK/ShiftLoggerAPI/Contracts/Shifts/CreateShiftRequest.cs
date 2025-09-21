namespace ShiftLoggerAPI.Contracts.Shifts;

public sealed record CreateShiftRequest(DateTime Start, DateTime End, int EmployeeId);