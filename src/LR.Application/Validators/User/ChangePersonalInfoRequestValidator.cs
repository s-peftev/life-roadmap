using FluentValidation;
using LR.Application.Requests.User;
using LR.Domain.Constants.Validation;

namespace LR.Application.Validators.User
{
    public class ChangePersonalInfoRequestValidator : AbstractValidator<ChangePersonalInfoRequest>
    {
        public ChangePersonalInfoRequestValidator() 
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(UserValidationRules.MaxNameLength)
                .WithMessage("FirstName cannot exceed " + UserValidationRules.MaxNameLength + " characters.")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(UserValidationRules.MaxNameLength)
                .WithMessage("LastName cannot exceed " + UserValidationRules.MaxNameLength + " characters.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.BirthDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("BirthDate cannot be in the future.")
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-UserValidationRules.MaxUserAgeYears)))
                .WithMessage("BirthDate is not valid.")
                .When(x => x.BirthDate.HasValue);
        }
    }
}
