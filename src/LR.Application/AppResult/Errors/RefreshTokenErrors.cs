using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.Errors
{
    public static class RefreshTokenErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(RefreshToken));

        public static readonly Error SaveFailed =
            ErrorFactory.SaveFailed(nameof(RefreshToken));
    }
}
