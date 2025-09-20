namespace ShiftLoggerAPI.Contracts;

public sealed record CreateEmployeeRequest(string Name, string Title);
public sealed record UpdateEmployeeRequest(string Name, string Title);
public sealed record EmployeeResponse(int Id, string Name, string Title);
