using System.Net;
using System.Text.Json;

namespace ShiftsLogger.MiddleWare;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger =  logger;

    public async Task InvokeAsync(HttpContext context) 
    {
        try 
        {
            await _next(context);
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    new
                    {
                        Message = "An unexpected error occurred"
                    }));
        }
    }
}