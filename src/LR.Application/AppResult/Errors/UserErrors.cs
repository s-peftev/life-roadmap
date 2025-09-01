using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
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
    }
}
