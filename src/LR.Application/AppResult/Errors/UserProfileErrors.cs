using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.Errors
{
    public static class UserProfileErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(UserProfile));

        public static readonly Error SaveFailed =
            ErrorFactory.SaveFailed(nameof(UserProfile));
    }
}
