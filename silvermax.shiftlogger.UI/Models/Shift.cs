namespace silvermax.shiftlogger.UI.Models;

public record Shift(
    Guid shiftId, string? name, DateTime startTime, DateTime endTime, TimeSpan duration);

