namespace LR.Application.AppResult.Errors.User
{
    public static class PhotoErrors
    {
        public static readonly Error PhotoUploadFailed = new(
            "PhotoUploadFailed",
            Domain.Enums.ErrorType.Business,
            "Failed to upload photo to storage.");

        public static readonly Error PhotoDeletionFailed = new(
            "PhotoDeletionFailed",
            Domain.Enums.ErrorType.Business,
            "Failed to delete photo from storage.");

        public static readonly Error ServiceUnavailable = new(
            "ServiceUnavailable",
            Domain.Enums.ErrorType.ServiceUnavailable,
            "Failed to connect with storage.");
    }
}
