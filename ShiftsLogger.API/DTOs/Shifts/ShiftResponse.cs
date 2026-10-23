public sealed record ShiftResponse(
    int Id,
    int EmployeeId,
    DateTime StartTime,
    DateTime EndTime,
    ShiftEmployeeResponse Employee)
{
    public TimeSpan Duration => EndTime - StartTime;
}

public sealed record ShiftEmployeeResponse(
    string FirstName,
    string LastName,
    string Role);
