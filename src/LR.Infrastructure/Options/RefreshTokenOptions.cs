namespace LR.Infrastructure.Options
{
    public class RefreshTokenOptions
    {
        public const string RefreshTokenOptionsKey = "RefreshTokenOptions";

        public string TokenName { get; set; } = null!;
        public int ExpirationTimeInDays { get; set; }
    }
}
