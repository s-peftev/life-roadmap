using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class PhotoErrors
    {
        public static readonly Error PhotoUploadFailed = new(
            UserErrorIDs.PhotoUploadFailed,
            ErrorType.Business,
            "Failed to upload photo to storage."
        );

        public static readonly Error PhotoDeletionFailed = new(
            UserErrorIDs.PhotoDeletionFailed,
            ErrorType.Business,
            "Failed to delete photo from storage."
        );
    }
}