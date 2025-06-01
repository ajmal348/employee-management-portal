using EmployeeManagement.API.Models.Responses;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _env.IsDevelopment()
                               ? new ApiErrorResponse
                               {
                                   StatusCode = context.Response.StatusCode,
                                   Message = ex.Message,
                                   StackTrace = ex.StackTrace
                               }
                               : new ApiErrorResponse
                               {
                                   StatusCode = context.Response.StatusCode,
                                   Message = "An unexpected error occurred."
                               };


                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
}
