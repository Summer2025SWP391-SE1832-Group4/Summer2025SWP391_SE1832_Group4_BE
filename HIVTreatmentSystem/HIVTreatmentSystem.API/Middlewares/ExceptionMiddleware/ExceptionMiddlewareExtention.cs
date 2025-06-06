namespace CinemaBooking.API.Middlewares
{
    /// <summary>
    /// Custom here to call on program.cs and must have "this IApplicationBuilder"
    /// </summary>
    public static class ExceptionMiddlewareExtensions 
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}