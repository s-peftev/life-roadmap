namespace LR.Application.Constants.ErrorIDs
{
    public static class UserErrorIDs
    {
        // user identity errors
        public const string EmailConfirmationFailed = "EmailConfirmationFailed";
        public const string EmailIsTaken = "EmailIsTaken";
        public const string InvalidChangePasswordRequest = "InvalidChangePasswordRequest";
        public const string InvalidEmailCodeRequest = "InvalidEmailCodeRequest";
        public const string InvalidForgotPasswordRequest = "InvalidForgotPasswordRequest";
        public const string InvalidLoginRequest = "InvalidLoginRequest";
        public const string InvalidRegisterRequest = "InvalidRegisterRequest";
        public const string LoginFailed = "LoginFailed";
        public const string PasswordResetFailed = "PasswordResetFailed";
        public const string UsernameIsTaken = "UsernameIsTaken";
        public const string WrongCurrentPassword = "WrongCurrentPassword";

        // profile errors
        public const string InvalidProfilePhotoUploadRequest = "InvalidProfilePhotoUploadRequest";
        public const string InvalidChangeUsernameRequest = "InvalidChangeUsernameRequest";
        public const string InvalidChangePersonalInfoRequest = "InvalidChangePersonalInfoRequest";

        // refresh token errors
        public const string RefreshTokenInvalid = "RefreshTokenInvalid";
        public const string TokenMissing = "TokenMissing";

        // user photo errors
        public const string PhotoUploadFailed = "PhotoUploadFailed";
        public const string PhotoDeletionFailed = "PhotoDeletionFailed";
        public const string ServiceUnavailable = "ServiceUnavailable";
    }
}
