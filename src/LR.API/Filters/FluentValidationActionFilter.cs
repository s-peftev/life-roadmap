using FluentValidation;
using LR.Application.AppResult.Errors;
using LR.Application.Interfaces.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LR.API.Filters
{
    public class FluentValidationActionFilter(IServiceProvider serviceProvider, IErrorResponseFactory errorResponseFactory) 
        : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CancellationToken ct = context.HttpContext.RequestAborted;

            foreach (var arg in context.ActionArguments.Values)
            { 
                if (arg is null)
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());

                if (serviceProvider.GetService(validatorType) is not IValidator validator)
                    continue;

                var result = await validator.ValidateAsync(new ValidationContext<object>(arg), ct);

                if (!result.IsValid) 
                {
                    var error = ValidationErrors.InvalidRequest with
                    {
                        Details = result.Errors.Select(e => e.ErrorMessage)
                    };

                    context.Result = errorResponseFactory.CreateErrorResponse(error);

                    return;
                }
            }

            await next();
        }
    }
}
