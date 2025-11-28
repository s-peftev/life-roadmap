using LR.Application.AppResult;
using LR.Application.Interfaces.Utils;
using LR.Application.Responses;
using LR.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LR.Infrastructure.Utils
{
    public class ErrorResponseFactory : IErrorResponseFactory
    {
        public IActionResult CreateErrorResponse(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(ApiResponse<object>.Fail(error)),
            ErrorType.Conflict => new ConflictObjectResult(ApiResponse<object>.Fail(error)),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(ApiResponse<object>.Fail(error)),
            ErrorType.NotFound => new NotFoundObjectResult(ApiResponse<object>.Fail(error)),
            ErrorType.Business => new UnprocessableEntityObjectResult(ApiResponse<object>.Fail(error)),
            ErrorType.InternalServerError => new ObjectResult(ApiResponse<object>.Fail(error)) { StatusCode = 500 },
            ErrorType.ServiceUnavailable => new ObjectResult(ApiResponse<object>.Fail(error)) { StatusCode = 503 },
            _ => new ObjectResult(ApiResponse<object>.Fail(error)) { StatusCode = 500 }
        };
    }
}
