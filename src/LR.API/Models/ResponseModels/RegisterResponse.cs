using LR.Domain.Entities.Users;
using LR.Persistance.Identity;

namespace LR.API.Models.ResponseModels
{
    public class RegisterResponse(AppUser user, UserProfile profile)
    {
        public string UserName { get; set; } = user.UserName;
        public Guid ProfileId { get; set; } = profile.Id;
        public string UserId { get; set; } = user.Id;
    }
}
