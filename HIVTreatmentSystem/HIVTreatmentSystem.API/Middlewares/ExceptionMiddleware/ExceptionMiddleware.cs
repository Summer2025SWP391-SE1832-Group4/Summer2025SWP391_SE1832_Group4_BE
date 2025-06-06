using System.Net;
using System.Text.Json;

namespace CinemaBooking.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// InvokeAsync TO TAKE ERROR
        /// </summary>
        /// <param name="context"></param>

        public async Task InvokeAsync(HttpContext context)
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
        /// <summary>
        /// HandleExceptionAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "An error has occurred.";
            var response = new 
            {
                message = message,
                error = exception.Message
            };

            // ✅ Sử dụng JsonSerializerOptions để tránh lỗi serialize
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}