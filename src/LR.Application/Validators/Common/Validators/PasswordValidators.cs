using System.Text.RegularExpressions;

namespace LR.Application.Validators.Common.Validators
{
    public static class PasswordValidators
    {
        public static bool ContainDigit(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"\d");

        public static bool ContainLowercase(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, "[a-z]");

        public static bool ContainUppercase(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, "[A-Z]");
    }
}
