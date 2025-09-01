namespace LR.Application.DTOs.Token
{
    public class RefreshTokenGenerationDto
    {
        public required string UserId { get; init; }
        public Guid? SessionId { get; init; }
        public string? UserAgent { get; init; }
        public string? IpAddress { get; init; }
        public required int ExpirationDays { get; init; }
    }
}
