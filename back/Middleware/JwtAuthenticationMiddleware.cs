using System.Security.Claims;
using back.Services;

namespace back.Middleware;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtAuthenticationMiddleware> _logger;

    private static readonly string[] ExcludedPaths = new[]
    {
        "/api/auth",
        "/swagger",
        "/v3/api-docs"
    };

    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        // Permitir peticiones OPTIONS (preflight de CORS) sin autenticación
        if (context.Request.Method == "OPTIONS")
        {
            await _next(context);
            return;
        }

        var path = context.Request.Path.Value ?? string.Empty;

        // Skip authentication for excluded paths
        if (ExcludedPaths.Any(excluded => path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await _next(context);
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var principal = jwtService.GetPrincipalFromToken(token);

        if (principal != null)
        {
            context.User = principal;
        }

        await _next(context);
    }
}

