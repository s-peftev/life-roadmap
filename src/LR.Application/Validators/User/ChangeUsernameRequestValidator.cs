using FluentValidation;
using LR.Application.Requests.User;
using LR.Domain.Constants.Validation;

namespace LR.Application.Validators.User
{
    public class ChangeUsernameRequestValidator : AbstractValidator<ChangeUsernameRequest>
    {
        public ChangeUsernameRequestValidator() 
        {
            RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("UserName is required.")
                    .Length(UserValidationRules.MinUsernameNameLength, UserValidationRules.MaxUsernameNameLength)
                    .WithMessage("UserName must be between " + UserValidationRules.MinUsernameNameLength +
                                " and " + UserValidationRules.MaxUsernameNameLength + " characters.");
        }
    }
}
