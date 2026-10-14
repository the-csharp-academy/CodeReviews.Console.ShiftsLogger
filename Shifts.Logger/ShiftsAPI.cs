using System.Configuration;
using System.Net.Http.Json;

namespace Shifts.Logger
{
    public static class ShiftsAPI
    {
        private static HttpClient _client;
        static ShiftsAPI()
        {
           _client = new();
            var baseAdress = ConfigurationManager.AppSettings["BaseAdress"];
            if (baseAdress == null)
                throw new ArgumentException("Unable to load configuration of base adress.");
            _client.BaseAddress = new Uri(baseAdress);
        }
        

        public static async Task<List<Shift>?> GetShifts()
        {
            return await _client.GetFromJsonAsync<List<Shift>>(_client.BaseAddress);
        }

        public static async Task<Shift?> GetShiftById(int id)
        {
            return await _client.GetFromJsonAsync<Shift>($"{id}");
        }

        public static async Task<Shift> CreateShift(Shift shift)
        {
            var response = await _client.PostAsJsonAsync<Shift>(_client.BaseAddress, shift);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<Shift>();
            if (created == null)
                throw new InvalidOperationException($"Error occures with creating shift {shift}");
            return created;
        }

        public static async Task<Shift> Update(int id, Shift shift)
        {
            var response = await _client.PutAsJsonAsync($"{id}", shift);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<Shift>();
            if (created == null)
                throw new InvalidOperationException($"Error occures with creating shift {shift}");
            return created;
        }

        public static async Task DeleteShift(int id)
        {
            await _client.DeleteAsync($"{id}");
        }
    }
}