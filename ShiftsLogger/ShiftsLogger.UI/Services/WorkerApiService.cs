using System;
using System.Net.Http.Json;
using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI.Services
{
    public class WorkerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBase;

        public WorkerApiService(HttpClient httpClient, string urlBase)
        {
            _httpClient = httpClient;
            _urlBase = urlBase;
        }

        public async Task<List<WorkerReadDTO>?> GetAllWorkersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlBase + "/api/workers");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<WorkerReadDTO>>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        public async Task<WorkerReadDTO?> GetWorkerByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlBase + $"/api/workers/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<WorkerReadDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        public async Task<WorkerReadDTO?> CreateWorkerAsync(WorkerDTO worker)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_urlBase + "/api/workers", worker);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<WorkerReadDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateWorkerAsync(int id, WorkerDTO worker)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_urlBase + $"/api/workers/{id}", worker);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteWorkerAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_urlBase + $"/api/workers/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }
        }
    }
}
