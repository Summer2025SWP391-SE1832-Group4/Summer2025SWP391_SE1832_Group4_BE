using System.Diagnostics;

namespace CinemaBooking.API.Middlewares.LoggingMiddleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// Invoke methods to handle logging
        /// </summary>
        /// <param name="context"></param>

        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await _next(context);
            stopWatch.Stop();  
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} - {stopWatch.ElapsedMilliseconds}ms");
        }
    
    }
    
}

