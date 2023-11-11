namespace TaxCalculator.API.Middleware.Models;

public sealed class ApiError
{
    public string Message { get; set; } = @"Something went wrong.";

    public string Details { get; set; }

    public string RequestId { get; set; }
}