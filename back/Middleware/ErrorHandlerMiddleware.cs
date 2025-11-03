
using System.Net;
using System.Text.Json;
using back.Exceptions;

namespace back.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new { message = error.Message };

            switch (error)
            {
                case ResourceNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case DuplicateResourceException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case InvalidTokenException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case InvalidOperationException: // This will catch our capacity error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(error, "An unhandled error occurred.");
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}
