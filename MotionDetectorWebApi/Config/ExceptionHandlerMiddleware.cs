using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MotionDetectorWebApi.Config
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var message = exception.Message;
            var details = string.Empty;

            if (exception.InnerException != null)
                details = exception.InnerException.Message;

            //TODO: handle other status codes?
            if (exception.GetType() == typeof(UnauthorizedAccessException))
            {
                context.Response.StatusCode = 401;
                _logger.LogDebug("Unauthorized", exception, exception.StackTrace);
            }
            else
            {
                context.Response.StatusCode = 500;
                _logger.LogWarning("Unexpected error", exception, exception.StackTrace);
            }

            var response = JsonConvert.SerializeObject(new { message, details });
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(response);
        }
    }
}
