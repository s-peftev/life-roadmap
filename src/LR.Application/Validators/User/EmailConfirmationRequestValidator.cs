using FluentValidation;
using LR.Application.Requests.User;

namespace LR.Application.Validators.User
{
    public class EmailConfirmationRequestValidator : AbstractValidator<EmailConfirmationRequest>
    {
        public EmailConfirmationRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
