using Newtonsoft.Json;
using Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.FilterException;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.WebAPI.Middlewares
{
    /// <summary>
    /// Custom middleware.
    /// </summary>
    public class CustomMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate next = next;

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private static Task HandleException(HttpContext context, Exception ex)
        {
            var exceptionHandle = new ErrorExceptionHandler();
            var (status, errorMessage) = exceptionHandle.HandleException(ex);
            string result = JsonConvert.SerializeObject(errorMessage);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;

            return context.Response.WriteAsync(result);
        }
    }
}
