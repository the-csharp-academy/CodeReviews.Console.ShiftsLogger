using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Api.Dtos;

/// <summary>Body accepted by POST and PUT.</summary>
/// <remarks>
/// The attributes must NOT use the <c>property:</c> target. On a positional record MVC throws
/// "validation metadata defined on property 'WorkerName' that will be ignored ... must be
/// associated with the constructor parameter", which surfaces as a 500 on every POST/PUT.
/// A bare attribute targets the constructor parameter, which is what the model validator reads.
/// </remarks>
public record ShiftRequestDto(
    [Required(ErrorMessage = "Worker name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Worker name must be 1-100 characters.")]
    string WorkerName,
    DateTime StartTime,
    DateTime EndTime);
