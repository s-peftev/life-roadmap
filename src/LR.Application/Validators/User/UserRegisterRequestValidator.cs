using FluentValidation;
using LR.Application.Requests;
using System.Text.RegularExpressions;

namespace LR.Application.Validators.User
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .Length(4, 20).WithMessage("UserName must be between 4 and 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.")
                .Must(ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("FirstName cannot exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("LastName cannot exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }

        private bool ContainDigit(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"\d");

        private bool ContainLowercase(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, "[a-z]");

        private bool ContainUppercase(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, "[A-Z]");
    }
}
