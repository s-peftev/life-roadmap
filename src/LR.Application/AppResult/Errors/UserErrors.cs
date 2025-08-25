using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound = new(
            "UserNotFound",
            ErrorType.NotFound,
            "User not found.");

        public static readonly Error UsernameIsTaken = new(
            "UsernameIsTaken",
            ErrorType.Conflict,
            "Username is taken.");

        public static readonly Error EmailIsTaken = new(
            "EmailIsTaken",
            ErrorType.Conflict,
            "Email is taken.");

        public static readonly Error RegistrationFailed = new(
            "RegistrationFailed",
            ErrorType.Validation,
            "User registration failed.");
    }
}
