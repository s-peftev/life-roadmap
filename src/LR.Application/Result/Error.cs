using LR.Domain.Enums;

namespace LR.Application.Result
{
    public class Error(string id, ErrorType type, string description)
    {
        public string Id { get; } = id;
        public ErrorType Type { get; } = type;
        public string Description { get; } = description;
    }
}
