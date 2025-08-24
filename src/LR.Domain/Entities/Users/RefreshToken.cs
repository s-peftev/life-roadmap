namespace LR.Domain.Entities.Users
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Token { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = null!;
    }
}
