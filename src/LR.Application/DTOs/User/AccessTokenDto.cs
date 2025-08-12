namespace LR.Application.DTOs.User
{
    public class AccessTokenDto(string tokenValue, DateTime expiresAtUtc)
    {
        public string TokenValue { get; set; } = tokenValue;
        public DateTime ExpiresAtUtc { get; set; } = expiresAtUtc;
    }
}
