namespace ShiftLogger.Domain.Validation.Errors;

public static class Errors
{
    public static readonly Error None = Error.None;

    // Data Not Found
    public static readonly Error QueryReturnedNull = new("SL40401", "The attempted query returned null.");
    public static readonly Error ShiftIdNotFound = new("SL40402", "The requested shift could not be found.");
    public static readonly Error ShiftsNotFoundForEmployeeId = new("SL40403", "No shifts found for the specified employee.");
    public static readonly Error ShiftCountNull = new("SL40404", "The shift count query returned null for the specified employee.");
    public static readonly Error EmployeeNotFound = new("SL40405", "The requested employee could not be found.");
    public static readonly Error ShiftOverlapReturnedNull = new("SL40406", "Shift overlap check returned null.");


    // Conflicts
    public static readonly Error NoChangesToUpdatedData = new("SL40901", "Updated data was the same as the original data.");
    public static readonly Error RecordMismatch = new("SL40902", "There is a mismatch between the data sent and the data returned.");
    public static readonly Error NoSaveData = new("SL40903", "Saved failed because no changes were detected.");
    public static readonly Error ShiftAlreadyExists = new("SL40904", "A shift with the requested data already exists.");
    public static readonly Error EmployeeAlreadyExists = new("SL40905", "An employee with that name already exists.");
    public static readonly Error EmployeeNameIsBlank = new("SL40906", "Employee first and last names cannot be blank.");
    public static readonly Error DateRangeError = new("SL40907", "The end date cannot precede the start date.");
    public static readonly Error ClockInTimePrecedesClockOutTime = new("SL40908", "The Clock In time cannot be on or after the Clock Out time.");
    public static readonly Error NewShiftOverlapsExistingShift = new("SL40909", "This shift cannot overlap an existing shift.");
    public static readonly Error CannotLogFutureShifts = new("SL40910", "A shift cannot be entered with a time greater than the current time.");


    // Unknown Errors
    public static readonly Error SaveFailed = new("SL50001", "Save failed for unknown reason.");
    public static readonly Error DeserializationError = new("SL50002", "Could not parse API response.");
}