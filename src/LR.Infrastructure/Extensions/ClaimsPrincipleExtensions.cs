using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LR.Infrastructure.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get user id from token");

            return userId;
        }
    }
}
