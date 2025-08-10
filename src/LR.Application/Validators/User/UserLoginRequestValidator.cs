using FluentValidation;
using LR.Application.Requests;

namespace LR.Application.Validators.User
{
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Length(4, 20);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 20);
        }
    }
}
