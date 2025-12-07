using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class ValidationErrors
    {
        public static readonly Error InvalidRequest = new(
            ValidationErrorIDs.InvalidRequest,
            ErrorType.Validation,
            "Request is invalid."
        );
    }
}
