namespace LR.Infrastructure.Options
{
    public class RefreshTokenOptions
    {
        public const string RefreshTokenOptionsKey = "RefreshTokenOptions";

        public int ExpirationTimeInDays { get; set; }
        public int RotationThresholdDays { get; set; }
    }
}
