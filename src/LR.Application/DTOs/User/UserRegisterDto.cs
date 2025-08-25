namespace LR.Application.DTOs.User
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
