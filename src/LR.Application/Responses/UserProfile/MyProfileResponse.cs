namespace LR.Application.Responses.UserProfile
{
    public class MyProfileResponse
    {
        public required string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
