using LR.Domain.Enums;

namespace LR.Application.AppResult.Errors
{
    public static class ErrorFactory
    {
        public static Error NotFound(string name, string? description = null) =>
            new($"{name}NotFound", ErrorType.NotFound, description ?? $"{name} not found.");
    }
}
