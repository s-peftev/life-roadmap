using FluentValidation;
using LR.Application.Requests.User;

namespace LR.Application.Validators.User
{
    public class ChangeUsernameRequestValidator : AbstractValidator<ChangeUsernameRequest>
    {
        public ChangeUsernameRequestValidator() 
        {
            RuleFor(x => x.NewUsername)
                    .NotEmpty().WithMessage("UserName is required.")
                    .Length(4, 20).WithMessage("UserName must be between 4 and 20 characters.");
        }
    }
}
