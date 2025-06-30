using System.Diagnostics.CodeAnalysis;

namespace HIVTreatmentSystem.Application.Common
{
    /// <summary>
    /// Result pattern implementation for Clean Architecture
    /// Đảm bảo tách biệt concerns và xử lý lỗi nhất quán
    /// </summary>
    public class Result<T>
    {
        private Result(bool isSuccess, T? value, string? error, string[]? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            ValidationErrors = validationErrors ?? Array.Empty<string>();
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string? Error { get; }
        public string[] ValidationErrors { get; }

        /// <summary>
        /// Tạo kết quả thành công
        /// </summary>
        public static Result<T> Success(T value) => new(true, value, null);

        /// <summary>
        /// Tạo kết quả thất bại với lỗi
        /// </summary>
        public static Result<T> Failure(string error) => new(false, default, error);

        /// <summary>
        /// Tạo kết quả thất bại với validation errors
        /// </summary>
        public static Result<T> ValidationFailure(params string[] errors) => 
            new(false, default, "Validation failed", errors);

        /// <summary>
        /// Tạo kết quả không tìm thấy
        /// </summary>
        public static Result<T> NotFound(string message = "Resource not found") => 
            new(false, default, message);
    }

    /// <summary>
    /// Result pattern cho operations không trả về data
    /// </summary>
    public class Result
    {
        private Result(bool isSuccess, string? error, string[]? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            ValidationErrors = validationErrors ?? Array.Empty<string>();
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }
        public string[] ValidationErrors { get; }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
        public static Result ValidationFailure(params string[] errors) => 
            new(false, "Validation failed", errors);
    }
} 