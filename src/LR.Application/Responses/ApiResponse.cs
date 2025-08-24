using LR.Application.Result;

namespace LR.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public Error? Error { get; init; }

        private ApiResponse(T? data, bool success, Error? error = null)
        {
            Data = data;
            Success = success;
            Error = error;
        }

        public static ApiResponse<T> Ok(T data) =>
            new(data, true);

        public static ApiResponse<T> Fail(Error error) =>
            new(default, false, error);

    }
}
