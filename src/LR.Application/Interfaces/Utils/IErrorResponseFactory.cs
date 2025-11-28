using LR.Application.AppResult;
using Microsoft.AspNetCore.Mvc;

namespace LR.Application.Interfaces.Utils
{
    public interface IErrorResponseFactory
    {
        IActionResult CreateErrorResponse(Error error);
    }
}
