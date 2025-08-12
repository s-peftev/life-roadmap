namespace LR.Application.Responses.User
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAtUtc { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAtUtc { get; set; }
        public string? Message { get; set; }
    }
}
