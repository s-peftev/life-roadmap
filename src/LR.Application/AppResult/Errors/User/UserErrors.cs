using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound("User");

        public static readonly Error UsernameIsTaken = new(
            "UsernameIsTaken",
            ErrorType.Conflict,
            "Username is taken.");

        public static readonly Error EmailIsTaken = new(
            "EmailIsTaken",
            ErrorType.Conflict,
            "Email is taken.");

        public static readonly Error LoginFailed = new(
            "LoginFailed",
            ErrorType.Unauthorized,
            "User login failed. Wrong username or password");

        public static readonly Error InvalidRegisterRequest = new(
            "InvalidRegisterRequest",
            ErrorType.Validation,
            "Register request is invalid.");

        public static readonly Error InvalidLoginRequest = new(
            "InvalidLoginRequest",
            ErrorType.Validation,
            "Login request is invalid.");

        public static readonly Error InvalidEmailCodeRequest = new(
            "InvalidEmailCodeRequest",
            ErrorType.Validation,
            "Request for email confirmation code is invalid.");

        public static readonly Error EmailConfirmationFailed = new(
            "EmailConfirmationFailed",
            ErrorType.Validation,
            "Email confirmation failed.");

        public static readonly Error InvalidForgotPasswordRequest = new(
            "InvalidForgotPasswordRequest",
            ErrorType.Validation,
            "Request for password reset is invalid.");

        public static readonly Error PasswordResetFailed = new(
            "PasswordResetFailed",
            ErrorType.Validation,
            "Password reset failed.");
    }
}
