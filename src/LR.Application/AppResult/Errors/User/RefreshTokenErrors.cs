using LR.Domain.Entities.Users;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(RefreshToken));

        public static readonly Error RefreshTokenInvalid = new(
            "RefreshTokenInvalid",
            ErrorType.Unauthorized,
            "Refresh token is invalid.");

        public static readonly Error TokenMissing = new(
            "TokenMissing",
            ErrorType.Validation,
            "Refresh token is missing.");
    }
}