using System.Net.Http.Json;
using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI.Services;

public class WorkerService
{
    private static readonly HttpClient _httpClient = new();

    public async Task<List<WorkerDto>?> GetAllWorkers()
    {
        var response = await _httpClient.GetFromJsonAsync<List<WorkerDto>>($"{Configuration.ApiUrl}/Worker");
        return response;
    }
    public async Task<bool> AddWorker(string name)
    { 
        var response = await _httpClient.PostAsJsonAsync($"{Configuration.ApiUrl}/Worker", new { Name = name });
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteWorker(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Configuration.ApiUrl}/Worker/{id}");
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateWorker(int id, string name)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Configuration.ApiUrl}/Worker/{id}", new { Name = name } );
        return response.IsSuccessStatusCode;
    }
    public async Task<WorkerDetailsDTO?> GetWorkerDetails(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<WorkerDetailsDTO>($"{Configuration.ApiUrl}/Worker/{id}/Details");
        return response;
    }
}