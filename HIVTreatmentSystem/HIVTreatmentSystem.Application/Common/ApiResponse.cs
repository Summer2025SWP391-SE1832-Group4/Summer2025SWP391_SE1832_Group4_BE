namespace HIVTreatmentSystem.Application.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse(string message = "", object? data = null)
        {
            Success = data != null || (message != null && !message.ToLower().Contains("error") && !message.ToLower().Contains("invalid") && !message.ToLower().Contains("failed"));
            Message = message;
            Data = data;
        }
    }
} 