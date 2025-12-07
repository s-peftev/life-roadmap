using System.Security.Claims;

namespace LR.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetAppUserId(this ClaimsPrincipal appUser)
        {
            return appUser.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("Cannot get appUserId from token");
        }
    }
}
