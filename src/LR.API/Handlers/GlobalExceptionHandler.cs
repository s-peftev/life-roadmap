using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace LR.API.Handlers
{
    public class GlobalExceptionHandler() : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
        {
            var (statusCode, error) = GetExceptionDetails(exception);

            httpContext.Response.StatusCode = (int)statusCode;
            var response = ApiResponse<object>.Fail(error);

            await httpContext.Response.WriteAsJsonAsync(response, ct);

            return true;
        }

        private static (HttpStatusCode statusCode, Error error) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                OperationCanceledException _ => (HttpStatusCode.NoContent, ExceptionErrors.RequestCancelled),
                TimeoutException _ => (HttpStatusCode.RequestTimeout, ExceptionErrors.Timeout),
                _ => (HttpStatusCode.InternalServerError, ExceptionErrors.Unexpected)
            };
        }
    }
}
