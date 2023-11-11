using System.Net;
using Newtonsoft.Json;
using TaxCalculator.API.Middleware.Models;

namespace TaxCalculator.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleAsync(context, ex);
        }
    }
    
    private Task HandleAsync(HttpContext context, Exception exception, string? message = null)
    {
        return HandleAsync(context, exception, HttpStatusCode.InternalServerError, message);
    }
    private Task HandleAsync(HttpContext context,
        Exception exception,
        HttpStatusCode statusCode,
        string? message = null)
    {
        object error = new ApiError
            {
                Message = message ?? exception.Message,
                Details = exception.ToString(),
                RequestId = context.Connection.Id,
            };

        var serializedMessage = JsonConvert.SerializeObject(error);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(serializedMessage);
    }
}