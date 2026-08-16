using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Exceptions;

namespace ShiftsLogger.MiddleWare;

internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception ocurred");
            
        var (statusCode, title)= exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            ValidationException => (StatusCodes.Status400BadRequest, "Invalid request"),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Concurrency conflict"),
            DbUpdateException => (StatusCodes.Status500InternalServerError, "Database error"),
            ApplicationException => (StatusCodes.Status400BadRequest, "Bad request"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")

        };

        if (statusCode >= 500)
            logger.LogError(exception, "Unhandled exception occurred");
        else
            logger.LogWarning(exception, "Handled exception occurred: {Message}", exception.Message);

        httpContext.Response.StatusCode = statusCode;


        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Type = exception.GetType().Name,
                Title = title,
                Detail = statusCode >= 500
                    ? "An internal error occurred. Please try again later."
                    : exception.Message
            }
        });
    }
}