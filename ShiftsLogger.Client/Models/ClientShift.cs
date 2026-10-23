public sealed class ClientShift
{
    public int Id { get; init; }
    public int EmployeeId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public ClientShiftEmployee Employee { get; init; } = null!;
    public TimeSpan Duration => EndTime - StartTime;
}

public sealed class ClientShiftEmployee
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}

public sealed record ShiftWriteRequest(int EmployeeID, DateTime StartTime, DateTime EndTime);
