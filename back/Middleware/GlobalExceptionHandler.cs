using System.Net;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using back.Exceptions;

namespace back.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        var response = new Dictionary<string, string>();

        switch (exception)
        {
            case ResourceNotFoundException:
                status = HttpStatusCode.NotFound;
                response["error"] = exception.Message;
                break;

            case DuplicateResourceException:
                status = HttpStatusCode.Conflict;
                response["error"] = exception.Message;
                break;

            case InvalidTokenException:
                status = HttpStatusCode.BadRequest;
                response["error"] = exception.Message;
                break;

            case UnauthorizedAccessException:
            case SecurityTokenException:
                status = HttpStatusCode.Unauthorized;
                response["error"] = "Credenciales inválidas.";
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                response["error"] = "Ha ocurrido un error interno del servidor.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

