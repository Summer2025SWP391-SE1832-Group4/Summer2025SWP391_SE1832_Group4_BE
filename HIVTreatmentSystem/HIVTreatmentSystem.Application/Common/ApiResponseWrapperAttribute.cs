using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HIVTreatmentSystem.Application.Common
{
    public class ApiResponseWrapperAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Skip if the result is already an ApiResponse
            if (context.Result is ObjectResult objectResult && objectResult.Value is ApiResponse)
            {
                return;
            }

            if (context.Result is ObjectResult result)
            {
                var statusCode = result.StatusCode ?? 200;
                var message = statusCode < 400 ? "Success" : "Error";
                
                context.Result = new ObjectResult(new ApiResponse(message, result.Value))
                {
                    StatusCode = statusCode
                };
            }
            
            base.OnResultExecuting(context);
        }
    }
} 