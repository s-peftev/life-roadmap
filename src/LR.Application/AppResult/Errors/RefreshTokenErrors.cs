using LR.Domain.Entities.Users;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(RefreshToken));

        public static readonly Error SaveFailed =
            ErrorFactory.SaveFailed(nameof(RefreshToken));

        public static readonly Error TokenMissing = new(
            "TokenMissing",
            ErrorType.Validation,
            "Refresh token is missing.");

        public static readonly Error TokenRevokingFailed = new(
            "TokenRevokingFailed",
            ErrorType.InternalServerError,
            "Failed to revoke the refresh token.");
    }
}
