using FluentValidation;
using LR.Application.Validators.User;
using Microsoft.Extensions.DependencyInjection;

namespace LR.Application.Validators
{
    public static class ValidatorsRegistration
    {
        public static void RegisterValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UserRegisterRequestValidator>();
        }
    }
}
