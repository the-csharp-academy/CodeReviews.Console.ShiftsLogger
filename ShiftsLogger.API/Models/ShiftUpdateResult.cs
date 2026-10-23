public enum ShiftUpdateStatus
{
    Updated,
    ShiftNotFound,
    InvalidEmployeeAssignment
}

public sealed record ShiftUpdateResult(ShiftUpdateStatus Status, Shift? Shift);
