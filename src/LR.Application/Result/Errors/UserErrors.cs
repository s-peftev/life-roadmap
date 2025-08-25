using LR.Domain.Enums;

namespace LR.Application.Result.Errors
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound = new("UserNotFound", ErrorType.NotFound, "User not found.");
        public static readonly Error UserAlreadyExists = new("UserAlreadyExists", ErrorType.Conflict, "User already exists.");
    }
}
