namespace ShiftLoggerAPI.Contracts.Shifts;

public sealed record UpdateShiftRequest(DateTime Start, DateTime End, int EmployeeId);