using LR.Application.Constants.ErrorIDs;
using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors.User
{
    public static class UserErrors
    {
        public static readonly Error UsernameIsTaken = new(
            UserErrorIDs.UsernameIsTaken,
            ErrorType.Conflict,
            "Username is taken."
        );

        public static readonly Error EmailIsTaken = new(
            UserErrorIDs.EmailIsTaken,
            ErrorType.Conflict,
            "Email is taken."
        );

        public static readonly Error LoginFailed = new(
            UserErrorIDs.LoginFailed,
            ErrorType.Unauthorized,
            "User login failed. Wrong username or password"
        );

        public static readonly Error EmailConfirmationFailed = new(
            UserErrorIDs.EmailConfirmationFailed,
            ErrorType.Business,
            "Email confirmation failed."
        );

        public static readonly Error PasswordResetFailed = new(
            UserErrorIDs.PasswordResetFailed,
            ErrorType.Business,
            "Password reset failed."
        );

        public static readonly Error WrongCurrentPassword = new(
            UserErrorIDs.WrongCurrentPassword,
            ErrorType.Business,
            "Wrong current password."
        );
    }
}