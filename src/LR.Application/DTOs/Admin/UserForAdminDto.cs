using LR.Domain.Enums;

namespace LR.Application.DTOs.Admin
{
    public class UserForAdminDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public required ICollection<Role> Roles { get; set; }
    }
}
