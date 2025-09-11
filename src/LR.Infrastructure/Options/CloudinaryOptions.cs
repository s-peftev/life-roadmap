namespace LR.Infrastructure.Options
{
    public class CloudinaryOptions
    {
        public const string CloudinaryOptionsKey = "CloudinaryOptions";

        public required string CloudName { get; set; }
        public required string ApiKey { get; set; }
        public required string ApiSecret { get; set; }
        public required string LibraryFolder { get; set; }
    }
}
