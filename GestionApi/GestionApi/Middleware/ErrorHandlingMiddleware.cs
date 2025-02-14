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
                _logger.LogError(
                    "Custom exception occurred in {Method} method with status code {StatusCode}. Error Message: {ErrorMessage}",
                    context.Request.Method,
                    ex.ErrorCode,
                    ex.Message
                );

                context.Response.StatusCode = ex.ErrorCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "An unexpected error occurred in {Method} method. Status code {StatusCode}. Error Message: {ErrorMessage}",
                    context.Request.Method,
                    500,
                    e.Message
                );

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(e.Message);

            }
        }
    }
}
