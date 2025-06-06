namespace CinemaBooking.API.Middlewares.LoggingMiddleware
{
    public static class LoggingMiddlewareExtention
    {
        public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingMiddleware>();
        }
    
    }
    
}

