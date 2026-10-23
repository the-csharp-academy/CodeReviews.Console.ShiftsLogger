using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ValidShiftTimeRangeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IShiftTimeRange shift)
        {
            throw new InvalidOperationException(
                $"{nameof(ValidShiftTimeRangeAttribute)} can only be applied to types implementing {nameof(IShiftTimeRange)}.");
        }

        if (shift.StartTime < shift.EndTime)
            return ValidationResult.Success;

        return new ValidationResult(
            ErrorMessage ?? "Start time must be earlier than or equal to end time.",
            [nameof(IShiftTimeRange.StartTime), nameof(IShiftTimeRange.EndTime)]);
    }
}

public interface IShiftTimeRange
{
    DateTime StartTime { get; }
    DateTime EndTime { get; }
}
