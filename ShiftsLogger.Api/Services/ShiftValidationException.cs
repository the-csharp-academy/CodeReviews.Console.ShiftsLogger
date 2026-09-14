namespace ShiftsLogger.Api.Services;

/// <summary>Thrown when a request breaks a business rule; the controller maps it to 400.</summary>
public class ShiftValidationException : Exception
{
    public ShiftValidationException(string message) : base(message) { }
}
