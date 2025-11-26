using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class ExceptionErrors
    {
        public static readonly Error RequestCancelled = new(
            ExceptionErrorIDs.RequestCancelled,
            ErrorType.None,
            "The request was canceled."
        );

        public static readonly Error Timeout = new(
            ExceptionErrorIDs.Timeout,
            ErrorType.InternalServerError,
            "The request timed out."
        );

        public static readonly Error Unexpected = new(
            ExceptionErrorIDs.UnexpectedError,
            ErrorType.InternalServerError,
            "An unexpected error occurred."
        );
    }
}
