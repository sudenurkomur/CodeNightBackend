using System.Net;
using System.Text.Json;
using FluentValidation;

namespace CodeNight.WebApi.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

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
        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Validation Error",
                    Errors = validationEx.Errors
                        .Select(e => new ErrorDetail { Field = e.PropertyName, Message = e.ErrorMessage })
                        .ToList()
                }),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not Found",
                    Errors = new List<ErrorDetail>
                    {
                        new() { Field = string.Empty, Message = exception.Message }
                    }
                }),
            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Errors = new List<ErrorDetail>
                    {
                        new() { Field = string.Empty, Message = "An unexpected error occurred." }
                    }
                })
        };

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public int Status { get; set; }
    public string Title { get; set; } = null!;
    public List<ErrorDetail> Errors { get; set; } = new();
}

public class ErrorDetail
{
    public string Field { get; set; } = null!;
    public string Message { get; set; } = null!;
}
