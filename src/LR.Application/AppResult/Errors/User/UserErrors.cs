using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            ErrorFactory.NotFound("User");

        public static readonly Error UsernameIsTaken = new(
            UserErrorIDs.UsernameIsTaken,
            ErrorType.Conflict,
            "Username is taken.");

        public static readonly Error EmailIsTaken = new(
            UserErrorIDs.EmailIsTaken,
            ErrorType.Conflict,
            "Email is taken.");

        public static readonly Error LoginFailed = new(
            UserErrorIDs.LoginFailed,
            ErrorType.Unauthorized,
            "User login failed. Wrong username or password");

        public static readonly Error InvalidRegisterRequest = new(
            UserErrorIDs.InvalidRegisterRequest,
            ErrorType.Validation,
            "Register request is invalid.");

        public static readonly Error InvalidLoginRequest = new(
            UserErrorIDs.InvalidLoginRequest,
            ErrorType.Validation,
            "Login request is invalid.");

        public static readonly Error InvalidEmailCodeRequest = new(
            UserErrorIDs.InvalidEmailCodeRequest,
            ErrorType.Validation,
            "Request for email confirmation code is invalid.");

        public static readonly Error EmailConfirmationFailed = new(
            UserErrorIDs.EmailConfirmationFailed,
            ErrorType.Validation,
            "Email confirmation failed.");

        public static readonly Error InvalidForgotPasswordRequest = new(
            UserErrorIDs.InvalidForgotPasswordRequest,
            ErrorType.Validation,
            "Request for password reset is invalid.");

        public static readonly Error PasswordResetFailed = new(
            UserErrorIDs.PasswordResetFailed,
            ErrorType.Validation,
            "Password reset failed.");

        public static readonly Error InvalidChangePasswordRequest = new(
            UserErrorIDs.InvalidChangePasswordRequest,
            ErrorType.Validation,
            "Request for password change is invalid.");

        public static readonly Error WrongCurrentPassword = new(
            UserErrorIDs.WrongCurrentPassword,
            ErrorType.Validation,
            "Wrong current password.");
    }
}