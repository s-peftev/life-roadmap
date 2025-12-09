namespace LR.Infrastructure.Options
{
    public class SeedOptions
    {
        public const string SeedOptionsKey = "SeedOptions";
        public bool EnableFakeUsers { get; set; }
        public int FakeUsersCount { get; set; }
    }
}
