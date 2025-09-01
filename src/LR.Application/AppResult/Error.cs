using LR.Domain.Enums;

namespace LR.Application.AppResult
{
    public record Error(
        string Id,
        ErrorType Type,
        string Description,
        IEnumerable<string>? Details = null);
}
