using System;
using System.Collections;
using System.Linq;

namespace HIVTreatmentSystem.Application.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse(string message = "Success", object data = null)
        {
            Data = WrapDataIfList(data);
            Message = message;
            Success = InferSuccess(message, data);
        }

        private bool InferSuccess(string message, object data)
        {
            if (data == null) return false;
            var msg = message?.ToLower() ?? string.Empty;

            return !msg.Contains("error") &&
                   !msg.Contains("invalid") &&
                   !msg.Contains("failed") &&
                   !msg.Contains("already exists") &&
                   !msg.Contains("already in use");
        }

        private object WrapDataIfList(object data)
        {
            if (data is IEnumerable enumerable && !(data is string))
            {
                var list = enumerable.Cast<object>().ToList();
                return new
                {
                    Count = list.Count,
                    Items = list
                };
            }

            return data ?? new { };
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(string message = "Success", T data = default)
        {
            Message = message;
            Data = data;
            Success = !message.ToLower().Contains("error")
                      && !message.ToLower().Contains("invalid")
                      && !message.ToLower().Contains("failed");
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>(message)
            {
                Success = false,
                Data = default
            };
        }
    }
}
