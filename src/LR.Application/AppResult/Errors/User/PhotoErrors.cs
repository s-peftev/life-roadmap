using LR.Application.Constants.ErrorIDs;

namespace LR.Application.AppResult.Errors.User
{
    public static class PhotoErrors
    {
        public static readonly Error PhotoUploadFailed = new(
            UserErrorIDs.PhotoUploadFailed,
            Domain.Enums.ErrorType.Business,
            "Failed to upload photo to storage."
        );

        public static readonly Error PhotoDeletionFailed = new(
            UserErrorIDs.PhotoDeletionFailed,
            Domain.Enums.ErrorType.Business,
            "Failed to delete photo from storage."
        );

        public static readonly Error ServiceUnavailable = new(
            UserErrorIDs.ServiceUnavailable,
            Domain.Enums.ErrorType.ServiceUnavailable,
            "Failed to connect with storage."
        );
    }
}