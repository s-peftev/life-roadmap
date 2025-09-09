using FluentValidation;
using LR.Application.Requests.User;
using LR.Application.Validators.Common.Validators;

namespace LR.Application.Validators.User
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator() 
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 20).WithMessage("Password must be between 8 and 20 characters.")
                .Must(PasswordValidators.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(PasswordValidators.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(PasswordValidators.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
