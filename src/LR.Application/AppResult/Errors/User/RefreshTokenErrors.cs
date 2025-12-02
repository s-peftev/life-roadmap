using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class RefreshTokenErrors
    {
        public static readonly Error RefreshTokenInvalid = new(
            UserErrorIDs.RefreshTokenInvalid,
            ErrorType.Unauthorized,
            "Refresh token is invalid."
        );

        public static readonly Error TokenMissing = new(
            UserErrorIDs.TokenMissing,
            ErrorType.Unauthorized,
            "Refresh token is missing."
        );
    }
}