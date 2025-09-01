using LR.Application.Constants.ExceptionMessages;

namespace LR.Application.AppResult
{
    public class Result
    {
        public bool IsSuccess { get; }
        private readonly Error? _error;
        public Error Error =>
        IsSuccess
            ? throw new InvalidOperationException(ResultExceptionMessages.CannotAccessErrorOnSuccess)
            : _error!;

        protected Result(bool isSuccess, Error? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException(ResultExceptionMessages.SuccessMustNotHaveError);
            if (!isSuccess && error == null)
                throw new InvalidOperationException(ResultExceptionMessages.FailureMustHaveError);

            IsSuccess = isSuccess;
            _error = error;
        }

        public static Result Success() => new(true, null);

        public static Result Failure(Error error) => new(false, error);

        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure) =>
            IsSuccess ? onSuccess() : onFailure(Error);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value =>
            IsSuccess
                ? _value!
                : throw new InvalidOperationException(ResultExceptionMessages.CannotAccessValueOnFailure);

        private Result(T value) : base(true, null)
        {
            _value = value 
                ?? throw new ArgumentNullException(nameof(value), ResultExceptionMessages.SuccessCannotHaveNullValue);
        }

        private Result(Error error) : base(false, error)
        {
            _value = default;
        }

        public static Result<T> Success(T value) => new(value);

        public static new Result<T> Failure(Error error) => new(error);

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
            IsSuccess ? onSuccess(Value) : onFailure(Error);
    }
}
