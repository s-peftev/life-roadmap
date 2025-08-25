namespace LR.Application.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException("A successful result must not have an error.");
            if (!isSuccess && error == null)
                throw new InvalidOperationException("A failed result must have an error.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);

        public static Result Failure(Error error) => new(false, error);

        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
            IsSuccess ? onSuccess() : onFailure(Error!);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true, null)
        {
            Value = value;
        }

        private Result(Error error) : base(false, error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new(value);

        public static new Result<T> Failure(Error error) => new(error);

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
            IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}
