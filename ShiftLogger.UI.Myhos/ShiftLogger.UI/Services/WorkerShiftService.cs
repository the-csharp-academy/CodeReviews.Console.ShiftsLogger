using ShiftLogger.UI.DTOs;
using ShiftLogger.UI.Model;
using Spectre.Console;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShiftLogger.UI.Services;

internal class WorkerShiftService
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://localhost:7138/api/WorkerShift/"),
    };

    internal async Task<WorkerShift> CreateWorkerShift(CreateWorkerShiftDTO workerShiftDTO)
    {
        try
        {
            var response = await sharedClient.PostAsJsonAsync("Create", workerShiftDTO);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                AnsiConsole.MarkupLine("[#FFF15C]The submitted data is invalid.[/]");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                AnsiConsole.MarkupLine("[#F0362C]Internal server error.[/]");
                return null;
            }

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            WorkerShift? result = JsonSerializer.Deserialize<WorkerShift>(json, options);

            return result;
        }
        catch (Exception ex)
        {
            HanldeException(ex);
            return null;
        }
    }

    internal async Task<string> DeleteWorkerShift(int shiftId)
    {
        try 
        {
            var response = await sharedClient.DeleteAsync($"Delete={shiftId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                AnsiConsole.MarkupLine("[#FFF15C]Worker shift not found.[/]");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                AnsiConsole.MarkupLine("[#F0362C]Internal server error.[/]");
                return null;
            }

            response.EnsureSuccessStatusCode();

            string message = await response.Content.ReadAsStringAsync();

            return message;
        }
        catch (Exception ex)
        {
            HanldeException(ex);
            return null;
        }
    }

    internal async Task<List<WorkerShift>> GetAllWorkerShifts()
    {
        try
        {
            var response = await sharedClient.GetAsync("All");

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                AnsiConsole.MarkupLine("[#F0362C]The server returned an internal error.[/]");
                return [];
            }

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<WorkerShift>? result = JsonSerializer.Deserialize<List<WorkerShift>>(json, options);

            return result;
        }
        catch (Exception ex)
        {
            HanldeException(ex);
            return null;
        }
    }

    internal async Task<WorkerShift> GetWorkerShiftById(string id)
    {
        try
        {
            var response = await sharedClient.GetAsync($"Get={id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                AnsiConsole.MarkupLine("[#FFF15C]Worker shift not found.[/]");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                AnsiConsole.MarkupLine("[#F0362C]Internal server error.[/]");
                return null;
            }

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            WorkerShift? result = JsonSerializer.Deserialize<WorkerShift>(json, options);

            return result;
        }
        catch (Exception ex)
        {
            HanldeException(ex);
            return null;
        }
    }

    internal async Task<WorkerShift> UpdateWorkerShift(int id, UpdateWorkerShiftDTO dto)
    {
        try
        {
            var response = await sharedClient.PatchAsJsonAsync($"Update={id}", dto);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                AnsiConsole.MarkupLine("[#FFF15C]Worker shift not found.[/]");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                AnsiConsole.MarkupLine("[#FFF15C]Invalid update request.[/]");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                AnsiConsole.MarkupLine("[#F0362C]Internal server error.[/]");
                return null;
            }

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            WorkerShift result = JsonSerializer.Deserialize<WorkerShift>(json, options);

            return result;
        }
        catch(Exception ex) 
        {
            HanldeException(ex);
            return null;
        }
    }

    public static void HanldeException(Exception ex) 
    {
        switch(ex)
        {
            case HttpRequestException:
                AnsiConsole.MarkupLine($"[#F0362C]Unable to connect to the API. Make sure it is running.[/]");
                break;
            case TaskCanceledException:
                AnsiConsole.MarkupLine("[#FFF15C]The request timed out.[/]");
                break;
            default:
                AnsiConsole.MarkupLine($"[#F0362C]Unexpected error: {ex.Message}[/]");
                break;
        }
    }
}
