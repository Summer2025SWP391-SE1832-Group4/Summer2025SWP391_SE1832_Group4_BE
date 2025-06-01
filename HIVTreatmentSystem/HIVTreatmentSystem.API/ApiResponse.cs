public class ApiResponse
{
    public string Message { get; set; }
    public object? Data { get; set; }

    public ApiResponse(string message, object? data = null)
    {
        Message = message;
        Data = data;
    }
} 