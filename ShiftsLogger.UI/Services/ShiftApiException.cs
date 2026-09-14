namespace ShiftsLogger.UI.Services;

/// <summary>Every failure the UI can hit while talking to the API, in one type.</summary>
public class ShiftApiException : Exception
{
    public ShiftApiException(string message) : base(message) { }

    public ShiftApiException(string message, Exception inner) : base(message, inner) { }
}
