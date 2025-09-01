namespace LR.Application.DTOs.Token
{
    public class AccessTokenDto(string tokenValue, DateTime expiresAtUtc)
    {
        public string TokenValue { get; set; } = tokenValue;
        public DateTime ExpiresAtUtc { get; set; } = expiresAtUtc;
    }
}
