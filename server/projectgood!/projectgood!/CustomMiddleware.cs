public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomMiddleware> _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
    {
        _next = next;
        _logger = logger;   
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Custom logic before request processing
        _logger.LogInformation("Request Incoming: " + context.Request.Path);

        // Call the next middleware
        await _next(context);

        // Custom logic after request processing
        _logger.LogInformation("Response Sent: " + context.Response.StatusCode);
    }
}