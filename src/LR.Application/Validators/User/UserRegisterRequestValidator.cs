using FluentValidation;
using LR.Application.Requests.User;
using LR.Application.Validators.Common.Validators;

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
                .Must(PasswordValidators.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(PasswordValidators.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(PasswordValidators.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.");

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
    }
}
