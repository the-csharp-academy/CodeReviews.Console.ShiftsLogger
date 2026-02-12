using System.ComponentModel;
using System.Net.Http.Json;
using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI.Services;

public class ShiftService
{
    private readonly HttpClient _httpClient = new();
    public async Task<bool> AddShift(ShiftCreationDto shiftCreationDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Configuration.ApiUrl}/Shift", shiftCreationDto);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<IReadOnlyList<ShiftDto>?> GetShiftsByWorkerId(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<WorkerDetailsDTO>($"{Configuration.ApiUrl}/Worker/{id}/Details");
        if (response is null) return null;
        var workerShifts = response.Shifts;
        return workerShifts;
    }

    public async Task<bool> UpdateShift(ShiftDto shiftDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Configuration.ApiUrl}/Shift/{shiftDto.Id}", new {shiftDto.StartTime, shiftDto.EndTime});
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteShift(int shiftId)
    {
        var response = await _httpClient.DeleteAsync($"{Configuration.ApiUrl}/Shift/{shiftId}");
        return response.IsSuccessStatusCode;
    }
}