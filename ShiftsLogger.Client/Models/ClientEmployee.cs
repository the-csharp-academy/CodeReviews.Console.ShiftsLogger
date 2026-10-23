public sealed class ClientEmployee
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}

public sealed record EmployeeWriteRequest(
    string FirstName,
    string LastName,
    string Role,
    bool IsActive = true);
