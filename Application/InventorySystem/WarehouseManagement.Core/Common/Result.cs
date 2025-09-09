namespace WarehouseManagement.Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public string Message { get; }

        public bool IsFailure => !IsSuccess;

        public bool Succeeded { get; set; }

        protected Result(bool isSuccess, string error, string message)
        {
            IsSuccess = isSuccess;
            Error = error;
            Message = message;
        }

        public static Result Success() => new Result(true, "", "");
        public static Result Success(string message) => new Result(true, message, "");
        public static Result Failure(string error) => new Result(false, error, "");
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(bool isSuccess, string error, string message, T value)
            : base(isSuccess, error, message)
        {
            Value = value;
        }
        public bool Succeeded { get; private set; }

        public static Result<T> Success(T value) => new Result<T>(true, "", "", value);
        public static new Result<T> Failure(string error) => new Result<T>(false, error, "", default);
    }
}
