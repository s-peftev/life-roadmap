using LR.Application.Responses;
using LR.Application.Result;
using LR.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        protected IActionResult HandleFailure(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => BadRequest(ApiResponse<object>.Fail(error)),
                ErrorType.Conflict => Conflict(ApiResponse<object>.Fail(error)),
                ErrorType.Unauthorized => Unauthorized(ApiResponse<object>.Fail(error)),
                ErrorType.NotFound => NotFound(ApiResponse<object>.Fail(error)),
                ErrorType.InternalServerError => StatusCode(500, ApiResponse<object>.Fail(error)),
                ErrorType.None => StatusCode(500, ApiResponse<object>.Fail(error)),
                _ => StatusCode(500, ApiResponse<object>.Fail(error))
            };
    }
}
