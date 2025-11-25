using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.Responses;
using LR.Infrastructure.Constants;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace LR.API.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
        : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken ct)
        {
            var (statusCode, error) = GetExceptionDetails(exception);

            if (exception is not OperationCanceledException)
            {
                var correlationId = httpContext.Items[HttpHeaders.XCorrelationId]?.ToString() 
                    ?? httpContext.TraceIdentifier;

                var loggerMessage = $"CorrelationId: {correlationId}, Message: {exception.Message}";

                _logger.LogError(exception, loggerMessage);
            }

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
