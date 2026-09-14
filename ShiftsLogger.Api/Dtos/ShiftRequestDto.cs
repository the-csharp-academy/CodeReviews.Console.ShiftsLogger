using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.Api.Dtos;

/// <summary>Body accepted by POST and PUT.</summary>
public record ShiftRequestDto(
    [property: Required(ErrorMessage = "Worker name is required.")]
    [property: StringLength(100, MinimumLength = 1, ErrorMessage = "Worker name must be 1-100 characters.")]
    string WorkerName,
    DateTime StartTime,
    DateTime EndTime);
