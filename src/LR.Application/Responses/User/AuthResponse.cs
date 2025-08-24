using LR.Application.DTOs.User;

namespace LR.Application.Responses.User
{
    public class AuthResponse
    {
        public required AccessTokenDto AccessToken { get; set; }
        public required UserDto User { get; set; }
    }
}
