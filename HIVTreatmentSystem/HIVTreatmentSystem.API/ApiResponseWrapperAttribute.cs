using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ApiResponseWrapperAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is ApiResponse) return;
            var wrapped = new ApiResponse("Thành công", objectResult.Value);
            context.Result = new ObjectResult(wrapped)
            {
                StatusCode = objectResult.StatusCode
            };
        }
        else if (context.Result is EmptyResult)
        {
            context.Result = new ObjectResult(new ApiResponse("Thành công"))
            {
                StatusCode = 200
            };
        }
    }
} 