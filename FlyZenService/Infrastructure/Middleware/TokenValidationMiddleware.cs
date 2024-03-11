namespace FlyZenService.Infrastructure.Middleware;

public class TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;

    public async Task InvokeAsync(HttpContext context)
    {
        var apiKey = _configuration["ApiToken"];

        if (!context.Request.Headers.TryGetValue("apiToken", out var token) || token != apiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}
