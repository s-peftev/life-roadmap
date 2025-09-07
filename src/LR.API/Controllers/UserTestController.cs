using AutoMapper;
using AutoMapper.QueryableExtensions;
using LR.Application.DTOs.User;
using LR.Application.Responses;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LR.API.Controllers
{
    [Authorize]
    public class UserTestController(
        UserManager<AppUser> userManager,
        IMapper _mapper) : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
        }
    }
}
