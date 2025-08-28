using LR.Application.Responses.User;
using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.ResultData.Account
{
    public class AuthResult
    {
        public required AuthResponse AuthResponse { get; init; }
        public required RefreshToken RefreshToken { get; init; }
    }
}
