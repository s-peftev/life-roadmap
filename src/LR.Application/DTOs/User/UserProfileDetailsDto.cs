namespace LR.Application.DTOs.User
{
    public class UserProfileDetailsDto
    {
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
