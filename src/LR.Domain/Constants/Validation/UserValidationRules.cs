namespace LR.Domain.Constants.Validation
{
    public static class UserValidationRules
    {
        public const int MinUsernameNameLength = 4;
        public const int MaxUsernameNameLength = 20;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 20;
        public const int MaxNameLength = 50;
        public const int MaxUserAgeYears = 120;
    }
}
