namespace LR.Application.DTOs.User
{
    public class TokenUserDto
    {
        public required string Id { get; init; }
        public required string UserName { get; init; }
        public string? Email { get; init; }
        public required IList<string> Roles { get; init; }
    }
}
