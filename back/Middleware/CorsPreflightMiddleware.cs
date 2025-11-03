namespace back.Middleware;

public class CorsPreflightMiddleware
{
    private readonly RequestDelegate _next;

    public CorsPreflightMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Manejar peticiones OPTIONS (preflight) directamente
        if (context.Request.Method == "OPTIONS")
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", context.Request.Headers["Origin"].ToString());
            context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
            context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync(string.Empty);
            return;
        }

        await _next(context);
    }
}

