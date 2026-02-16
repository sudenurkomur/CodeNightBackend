using System.Text.Json;
using System.Text.Json.Serialization;
using CodeNight.Application.Common;
using FluentValidation;

namespace CodeNight.WebApi.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var requestId = context.TraceIdentifier;

        var (statusCode, errorResponse) = exception switch
        {
            ValidationException validationEx => (
                StatusCodes.Status400BadRequest,
                new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Code = ErrorCodes.ValidationError,
                        Message = "Validation failed.",
                        Details = validationEx.Errors.Select(e => new ApiErrorDetail
                        {
                            Field = e.PropertyName,
                            Issue = e.ErrorMessage
                        }).ToList(),
                        RequestId = requestId
                    }
                }
            ),
            KeyNotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Code = ErrorCodes.NotFound,
                        Message = notFoundEx.Message,
                        Details = new List<ApiErrorDetail>(),
                        RequestId = requestId
                    }
                }
            ),
            InvalidOperationException conflictEx when conflictEx.Message.Contains("conflict", StringComparison.OrdinalIgnoreCase) => (
                StatusCodes.Status409Conflict,
                new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Code = ErrorCodes.Conflict,
                        Message = conflictEx.Message,
                        Details = new List<ApiErrorDetail>(),
                        RequestId = requestId
                    }
                }
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                new ApiErrorResponse
                {
                    Error = new ApiError
                    {
                        Code = ErrorCodes.InternalError,
                        Message = "An unexpected error occurred.",
                        Details = new List<ApiErrorDetail>(),
                        RequestId = requestId
                    }
                }
            )
        };

        _logger.LogError(exception,
            "Request {RequestId} failed with status {StatusCode}: {Message}",
            requestId, statusCode, exception.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await context.Response.WriteAsync(json);
    }
}
