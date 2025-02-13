using GestionApi.Exceptions;

namespace GestionApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continuar con la solicitud
                await _next(context);
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred at {Time} in {MethodName}. Exception Details: {Message}", DateTime.UtcNow, nameof(InvokeAsync), ex.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(ex);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unhandled exception occurred at {Time} in {MethodName}. Exception Details: {Message}", DateTime.UtcNow, nameof(InvokeAsync), e.Message);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
            }
        }
    }
}
