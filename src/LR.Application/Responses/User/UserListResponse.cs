using LR.Application.DTOs.User;

namespace LR.Application.Responses.User
{
    public class UserListResponse(IEnumerable<UserDto> users)
    {
        public IEnumerable<UserDto> Users { get; set; } = users;
    }
}
