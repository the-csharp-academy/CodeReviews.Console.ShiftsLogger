using Newtonsoft.Json;
using ShiftsLoggerUI.Models;

namespace ShiftsLoggerUI.Services
{
    internal class ShiftsService
    {
        private static readonly HttpClient client = new HttpClient();

        static ShiftsService()
        {
            client.BaseAddress = new Uri("http://localhost:7064/");
            client.Timeout = TimeSpan.FromSeconds(30);
        }

        internal async Task ShowAllShifts(bool stay = true)
        {
            try
            {
                string shifts = await client.GetStringAsync("api/shifts");

                if (string.IsNullOrEmpty(shifts))
                {
                    Console.WriteLine("No shifts found.");
                    return;
                }

                var deserializedShift = JsonConvert.DeserializeObject<List<Shift>>(shifts);

                if (deserializedShift != null && deserializedShift.Count > 0)
                {
                    Menus.Visualizations.ShowAllShiftsTable(deserializedShift, stay);
                }
                else
                {
                    Console.WriteLine("No shifts found.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                Console.WriteLine("Tip: Verify the API is running at http://localhost:7064");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        internal async Task LogShift(Shift shift)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(shift);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/shifts", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Shift logged successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to log shift. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
                Console.WriteLine("Tip: Verify the API is running at http://localhost:7064");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        internal async Task UpdateShift(int id, Shift shift)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(shift);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/shifts/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Shift updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to update shift. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        internal async Task DeleteShift(int shiftId)
        {
            try
            {
                var response = await client.DeleteAsync($"api/shifts/{shiftId}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Shift deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to delete shift. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Serialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        internal static async Task<Shift> GetShiftById(int id)
        {
            try
            {
                string shift = await client.GetStringAsync($"api/shifts/{id}");
                var deserializedShift = JsonConvert.DeserializeObject<Shift>(shift);
                return deserializedShift!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching shift: {ex.Message}");
                return null!;
            }
        }
    }
    }