namespace HIVTreatmentSystem.Application.Common
{
    /// <summary>
    /// Validation result để xử lý validation logic tách biệt
    /// Tuân thủ Single Responsibility Principle
    /// </summary>
    public class ValidationResult
    {
        private readonly List<string> _errors = new();

        public bool IsValid => !_errors.Any();
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        public ValidationResult AddError(string error)
        {
            _errors.Add(error);
            return this;
        }

        public ValidationResult AddErrorIf(bool condition, string error)
        {
            if (condition)
                _errors.Add(error);
            return this;
        }

        public Result<T> ToResult<T>() where T : class
        {
            return IsValid 
                ? Result<T>.Success(default!) 
                : Result<T>.ValidationFailure(_errors.ToArray());
        }

        public Result ToResult()
        {
            return IsValid 
                ? Result.Success() 
                : Result.ValidationFailure(_errors.ToArray());
        }
    }
} 