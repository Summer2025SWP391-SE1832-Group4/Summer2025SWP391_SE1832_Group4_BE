using HIVTreatmentSystem.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Extensions
{
    /// <summary>
    /// Extension methods cho Result pattern to HTTP Response conversion
    /// Đảm bảo consistent response formatting theo Clean Architecture
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Convert Result<T> to appropriate HTTP response
        /// </summary>
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            if (result.IsSuccess)
            {
                return controller.Ok(new ApiResponse("Success", result.Value));
            }

            // Handle different types of failures
            if (result.ValidationErrors.Any())
            {
                return controller.BadRequest(new ApiResponse(result.Error ?? "Validation failed", result.ValidationErrors));
            }

            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return controller.NotFound(new ApiResponse(result.Error));
            }

            if (result.Error?.Contains("conflict", StringComparison.OrdinalIgnoreCase) == true ||
                result.Error?.Contains("already exists", StringComparison.OrdinalIgnoreCase) == true)
            {
                return controller.Conflict(new ApiResponse(result.Error));
            }

            return controller.BadRequest(new ApiResponse(result.Error ?? "An error occurred"));
        }

        /// <summary>
        /// Convert Result<T> to CreatedAtAction response
        /// </summary>
        public static IActionResult ToCreatedResult<T>(this Result<T> result, 
            ControllerBase controller, 
            string actionName, 
            object routeValues, 
            string successMessage = "Created successfully")
        {
            if (result.IsSuccess)
            {
                return controller.CreatedAtAction(
                    actionName, 
                    routeValues, 
                    new ApiResponse(successMessage, result.Value));
            }

            return result.ToActionResult(controller);
        }

        /// <summary>
        /// Convert Result (non-generic) to appropriate HTTP response
        /// </summary>
        public static IActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            if (result.IsSuccess)
            {
                return controller.Ok(new ApiResponse("Success"));
            }

            // Handle different types of failures
            if (result.ValidationErrors.Any())
            {
                return controller.BadRequest(new ApiResponse(result.Error ?? "Validation failed", result.ValidationErrors));
            }

            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return controller.NotFound(new ApiResponse(result.Error));
            }

            return controller.BadRequest(new ApiResponse(result.Error ?? "An error occurred"));
        }
    }
} 