using LR.Application.Constants.ErrorIDs;
using LR.Domain.Entities.Users;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(RefreshToken));

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