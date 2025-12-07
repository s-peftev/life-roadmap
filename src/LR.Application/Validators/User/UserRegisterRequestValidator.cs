using FluentValidation;
using LR.Application.Requests.User;
using LR.Application.Validators.Common.Validators;
using LR.Domain.Constants.Validation;

namespace LR.Application.Validators.User
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .Length(UserValidationRules.MinUsernameNameLength, UserValidationRules.MaxUsernameNameLength)
                .WithMessage("UserName must be between " + UserValidationRules.MinUsernameNameLength +
                             " and " + UserValidationRules.MaxUsernameNameLength + " characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(UserValidationRules.MinPasswordLength, UserValidationRules.MaxPasswordLength)
                .WithMessage("Password must be between " + UserValidationRules.MinPasswordLength +
                            " and " + UserValidationRules.MaxPasswordLength + " characters.")
                .Must(PasswordValidators.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(PasswordValidators.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(PasswordValidators.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.FirstName)
                .MaximumLength(UserValidationRules.MaxNameLength)
                .WithMessage("FirstName cannot exceed " + UserValidationRules.MaxNameLength + " characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(UserValidationRules.MaxNameLength)
                .WithMessage("LastName cannot exceed " + UserValidationRules.MaxNameLength + " characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
