using System.Text;
using System.Text.Json;

namespace ShiftsLoggerApiClient
{
    internal class ApiConnection
    {
        private readonly HttpClient client = new HttpClient();

        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<ApiResult> GetShifts()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7216/Shifts");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var shifts = JsonSerializer.Deserialize<IEnumerable<ShiftsLog>>(result, options);
                    return new ApiResult
                    {
                        Shifts = shifts,
                        Success = true
                    };
                }
                else
                {
                    var status = response.StatusCode;
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(status);
                    Console.WriteLine(error);
                    return new ApiResult
                    {
                        Status = status,
                        Success = false
                    };
                }
            }
            catch (HttpRequestException error)
            {
                Console.WriteLine(error);
                return new ApiResult
                {
                    Success = false
                };
            }
        }

        public async Task<ApiResult> CreateShift(ShiftDto newShift)
        {
            try
            {
                var shift = JsonSerializer.Serialize(newShift);
                StringContent content = new StringContent(
                    shift,
                    Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:7216/Shifts", content);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult
                    {
                        Success = true
                    };
                }
                else
                {
                    var status = response.StatusCode;
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(status);
                    Console.WriteLine(error);
                    return new ApiResult
                    {
                        Status = status,
                        Success = false
                    };
                }
            }
            catch (HttpRequestException error)
            {
                Console.WriteLine(error);
                return new ApiResult
                {
                    Success = false
                };
            }
        }

        public async Task<ApiResult> UpdateShift(int id, ShiftDto newShift)
        {
            try
            {
                var shift = JsonSerializer.Serialize(newShift);
                StringContent content = new StringContent(
                    shift,
                    Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await client.PutAsync($"https://localhost:7216/Shifts/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult
                    {
                        Success = true
                    };
                }
                else
                {
                    var status = response.StatusCode;
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(status);
                    Console.WriteLine(error);
                    return new ApiResult
                    {
                        Status = status,
                        Success = false
                    };
                }
            }
            catch (HttpRequestException error)
            {
                Console.WriteLine(error);
                return new ApiResult
                {
                    Success = false
                };
            }
        }

        public async Task<ApiResult> DeleteShift(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7216/Shifts/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult
                    {
                        Success = true
                    };
                }
                else
                {
                    var status = response.StatusCode;
                    Console.WriteLine(status);
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(error);
                    return new ApiResult
                    {
                        Status = status,
                        Success = false
                    };
                }
            }
            catch (HttpRequestException error)
            {
                Console.WriteLine(error);
                return new ApiResult
                {
                    Success = false
                };
            }
        }
    }
}