using silvermax.shiftlogger.UI.Models;
using Spectre.Console;

namespace silvermax.shiftlogger.UI;

internal class ShiftManager
{
    private readonly ShiftClient _client;

    public ShiftManager(ShiftClient shiftClient) => _client = shiftClient;

    public async Task<List<Shift>> GetAllShifts() => await _client.GetAllShifts();

    public async Task<Shift> CreateNewShift(CreateNewShiftDto dto) => await _client.CreateNewShift(dto);

    public async Task<string> DeleteShift(Guid id) => await _client.DeleteShift(id);

    public async Task<Shift> UpdateShift(Guid id, Shift updatedShift) => await _client.UpdateShift(id, updatedShift);
}
