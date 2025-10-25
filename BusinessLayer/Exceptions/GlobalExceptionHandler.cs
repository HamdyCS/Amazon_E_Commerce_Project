using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //log error
            string message = $"{exception.Message}";
            _logger.LogError(exception, message);


            //get status code
            int statusCode = 500;

            if (exception is ArgumentException)
                statusCode = StatusCodes.Status400BadRequest;

            if (exception is UnauthorizedAccessException)
                statusCode = StatusCodes.Status401Unauthorized;

            if (exception is ArgumentNullException)
                statusCode = StatusCodes.Status400BadRequest;


            if (exception is KeyNotFoundException)
                statusCode = StatusCodes.Status404NotFound;

            //update response
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Error = new
                {
                    Message = message,
                    StatusCode = statusCode,
                    time = DateTime.UtcNow
                }
            }, cancellationToken);


            //return true
            return true;
        }
    }
}
