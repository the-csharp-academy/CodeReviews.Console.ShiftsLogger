using ShiftsLogger.UI.Models;
using ShiftsLogger.UI.Services;

namespace ShiftsLogger.Tests.Ui;

/// <summary>Hand-written fake: lets the menu be tested without a server.</summary>
public class FakeShiftApiClient : IShiftApiClient
{
    public List<ShiftDto> Shifts { get; } = new();
    public List<ShiftRequest> Created { get; } = new();
    public bool ThrowUnreachable { get; set; }
    public int NextId { get; set; } = 1;

    /// <summary>Returns the exception to fail with, or null to succeed.</summary>
    private Exception? Fail() =>
        ThrowUnreachable
            ? new ShiftApiException("Could not reach the Shifts API. Make sure it is running, then try again.")
            : null;

    private Task<T> Respond<T>(Func<T> onSuccess)
    {
        var failure = Fail();
        return failure is null ? Task.FromResult(onSuccess()) : Task.FromException<T>(failure);
    }

    public Task<List<ShiftDto>> GetAllAsync(CancellationToken ct = default) =>
        Respond(() => Shifts.ToList());

    public Task<ShiftDto?> GetByIdAsync(int id, CancellationToken ct = default) =>
        Respond<ShiftDto?>(() => Shifts.FirstOrDefault(s => s.Id == id));

    public Task<ShiftDto> CreateAsync(ShiftRequest request, CancellationToken ct = default) =>
        Respond(() =>
        {
            Created.Add(request);
            var dto = new ShiftDto(NextId++, request.WorkerName, request.StartTime, request.EndTime);
            Shifts.Add(dto);
            return dto;
        });

    public Task<ShiftDto?> UpdateAsync(int id, ShiftRequest request, CancellationToken ct = default) =>
        Respond<ShiftDto?>(() =>
        {
            var updated = new ShiftDto(id, request.WorkerName, request.StartTime, request.EndTime);
            var index = Shifts.FindIndex(s => s.Id == id);
            if (index < 0)
            {
                return null;
            }

            Shifts[index] = updated;
            return updated;
        });

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
        Respond(() => Shifts.RemoveAll(s => s.Id == id) > 0);
}
