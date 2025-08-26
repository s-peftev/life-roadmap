using LR.Application.Responses.User;
using LR.Domain.Entities.Users;

namespace LR.Application.AppResult.ResultData.Account
{
    public class RegisterResultData
    {
        public required AuthResponse AuthResponse { get; init; }
        public required RefreshToken RefreshToken { get; init; }
    }
}
