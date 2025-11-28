using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.Errors.User
{
    public static class UserProfileErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(UserProfile));
    }
}