namespace LR.Application.DTOs.Token
{
    public class JwtGenerationDto
    {
        public required string UserId { get; init; }
        public required string UserName { get; init; }
        public string? Email { get; init; }
        public required IList<string> Roles { get; init; }
    }
}
