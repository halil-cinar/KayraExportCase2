using KayraExportCase2.Application.Result;
using System.Net;
using System.Text.Json;

namespace KayraExportCase2.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (UserException ex)
            {
                await HandleUserExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleSystemExceptionAsync(context, ex);
            }
        }

        private Task HandleUserExceptionAsync(HttpContext context, UserException ex)
        {
            var result = new SystemResult<object>();
            result.AddMessage(ex.Message, EPriority.Error);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }

        private Task HandleSystemExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");

            var result = new SystemResult<object>();
            result.AddDefaultErrorMessage(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
