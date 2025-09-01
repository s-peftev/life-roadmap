using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class ExceptionErrors
    {
        public static readonly Error RequestCancelled = new(
            "RequestCancelled",
            ErrorType.None,
            "The request was canceled.");

        public static readonly Error Timeout = new(
            "Timeout",
            ErrorType.InternalServerError,
            "The request timed out.");

        public static readonly Error Unexpected = new(
            "UnexpectedError",
            ErrorType.InternalServerError,
            "An unexpected error occurred.");
    }
}
