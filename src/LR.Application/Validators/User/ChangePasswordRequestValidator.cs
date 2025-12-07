using FluentValidation;
using LR.Application.Requests.User;
using LR.Application.Validators.Common.Validators;
using LR.Domain.Constants.Validation;

namespace LR.Application.Validators.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .Length(UserValidationRules.MinPasswordLength, UserValidationRules.MaxPasswordLength)
                .WithMessage("Password must be between " + UserValidationRules.MinPasswordLength +
                            " and " + UserValidationRules.MaxPasswordLength + " characters.")
                .Must(PasswordValidators.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(PasswordValidators.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(PasswordValidators.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.");
        }
    }
}
