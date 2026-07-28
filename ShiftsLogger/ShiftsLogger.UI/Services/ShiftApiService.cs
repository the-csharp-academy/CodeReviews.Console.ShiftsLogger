using ShiftsLogger.UI.DTOs;
using System;
using System.Net.Http.Json;

namespace ShiftsLogger.UI.Services
{
    public class ShiftApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBase;

        public ShiftApiService(HttpClient httpClient, string urlBase)
        {
            _httpClient = httpClient;
            _urlBase = urlBase;
        }

        public async Task<List<ShiftReadDTO>?> GetAllShiftsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlBase + "/api/shifts");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ShiftReadDTO>>();
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

        public async Task<ShiftReadDTO?> GetShiftByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlBase + $"/api/shifts/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ShiftReadDTO>();
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

        public async Task<List<ShiftReadDTO>?> GetShiftsByWorkerAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_urlBase + $"/api/workers/{id}/shifts");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ShiftReadDTO>>();
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

        public async Task<ShiftReadDTO?> CreateShiftAsync(ShiftCreateDTO shift)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_urlBase + "/api/shifts", shift);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ShiftReadDTO>();
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

        public async Task<bool> UpdateShiftAsync(int id, ShiftCreateDTO shift)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_urlBase + $"/api/shifts/{id}", shift);
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

        public async Task<bool> DeleteShiftAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_urlBase + $"/api/shifts/{id}");
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
