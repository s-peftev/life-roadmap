namespace LR.Infrastructure.Constants.ExceptionMessages
{
    public static class TokensExceptionMessages
    {
        public const string UserIdRequiredForJwt = "UserId is required for JWT generation.";
        public const string UserNameRequiredForJwt = "UserName is required for JWT generation.";
        public const string RolesRequiredForJwt = "At least one role is required for JWT generation.";
        public const string UserIdRequiredForRefreshToken = "UserId is required for refresh token generation.";
        public const string ExpirationDaysPositiveRequired = "ExpirationDays must be greater than zero for refresh token generation.";
    }
}
