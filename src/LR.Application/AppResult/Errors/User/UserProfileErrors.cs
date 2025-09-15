using LR.Domain.Entities.Users;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class UserProfileErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound(nameof(UserProfile));

        public static readonly Error InvalidProfilePhotoUploadRequest = new(
            "InvalidProfilePhotoUploadRequest",
            ErrorType.Validation,
            "Profile photo upload request is invalid.");

        public static readonly Error InvalidChangeUsernameRequest = new(
            "InvalidChangeUsernameRequest",
            ErrorType.Validation,
            "Change username request is invalid.");
    }
}
