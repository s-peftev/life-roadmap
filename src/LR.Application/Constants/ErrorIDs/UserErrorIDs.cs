namespace LR.Application.Constants.ErrorIDs
{
    public static class UserErrorIDs
    {
        // user identity errors
        public const string EmailIsTaken = "EmailIsTaken";
        public const string LoginFailed = "LoginFailed";
        public const string UsernameIsTaken = "UsernameIsTaken";
        public const string EmailConfirmationFailed = "EmailConfirmationFailed";
        public const string PasswordResetFailed = "PasswordResetFailed";
        public const string WrongCurrentPassword = "WrongCurrentPassword";

        // refresh token errors
        public const string RefreshTokenInvalid = "RefreshTokenInvalid";
        public const string TokenMissing = "TokenMissing";

        // user photo errors
        public const string PhotoUploadFailed = "PhotoUploadFailed";
        public const string PhotoDeletionFailed = "PhotoDeletionFailed";
    }
}
