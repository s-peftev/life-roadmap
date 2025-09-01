
namespace LR.Application.DTOs.User
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
