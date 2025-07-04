
namespace HIVTreatmentSystem.Application.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse(string message = "", object? data = null)
        {
            Success = data != null || (message != null && 
                                       !message.ToLower().Contains("error") && 
                                       !message.ToLower().Contains("invalid") && 
                                       !message.ToLower().Contains("failed") &&
                                       !message.ToLower().Contains("already exists") &&
                                       !message.ToLower().Contains("already in use"));
            Message = message;
            Data = data;
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(string message, T data = default)
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Data = default;
        }
    }
}