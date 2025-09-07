using FluentValidation;
using LR.Application.Requests.User;

namespace LR.Application.Validators.User
{
    public class EmailCodeRequestValidator : AbstractValidator<EmailCodeRequest>
    {
        public EmailCodeRequestValidator() 
        {
            RuleFor(x => x.UserName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
