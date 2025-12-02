using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class GeneralErrors
    {
        public static readonly Error NotFound = new(
            GeneralErrorIDs.NotFound,
            ErrorType.NotFound,
            "Entity not found."
        );

        public static readonly Error ServiceUnavailable = new(
            GeneralErrorIDs.ServiceUnavailable,
            ErrorType.ServiceUnavailable,
            "Service is temporarily unavailable."
        );

        public static readonly Error InternalServerError = new(
            GeneralErrorIDs.InternalServerError,
            ErrorType.InternalServerError,
            "Something went wrong."
        );
    }
}
