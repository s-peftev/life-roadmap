using AutoMapper;
using LR.API.Models.ResponseModels;
using LR.Application.DTOs.User;
using LR.Domain.Entities.Users;
using LR.Persistance.Identity;
using LR.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace LR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(
        AppDbContext _context,
        UserManager<AppUser> _userManager,
        IMapper _mapper
        ) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] UserRegisterDto request)
        {
            if (await IsUserExistsAsync(request.UserName!))
                return BadRequest("The username is taken.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            var user = _mapper.Map<AppUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            var profile = _mapper.Map<UserProfile>(request);
            profile.UserId = user.Id;

            _context.Add(profile);

            try
            {
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new RegisterResponse(user, profile));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                await _userManager.DeleteAsync(user);

                return BadRequest("Failed to save user profile.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<UserListResponse>> GetUsers()
        {
            var users = await _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();

            return Ok(new UserListResponse(users));
        }

        private async Task<bool> IsUserExistsAsync(string userName)
            => await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
    }
}
