using LR.Application.DTOs.User;

namespace LR.API.Models.ResponseModels
{
    public class UserListResponse(IEnumerable<UserDto> users)
    {
        public IEnumerable<UserDto> Users { get; set; } = users;
    }
}
