using GestionApi.Exceptions;

namespace GestionApi.Middleware
{
    public class LogRequestsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestsMiddleware> _logger;

        public LogRequestsMiddleware(RequestDelegate next, ILogger<LogRequestsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Request started - {RequestName} with {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            _logger.LogInformation("Request finished - {RequestName} with {Path} and {StatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}
