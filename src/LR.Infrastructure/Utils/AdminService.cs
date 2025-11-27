using AutoMapper;
using AutoMapper.QueryableExtensions;
using LR.Application.AppResult;
using LR.Application.DTOs.Admin;
using LR.Application.Interfaces.Utils;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LR.Infrastructure.Utils
{
    public class AdminService(UserManager<AppUser> userManager, IMapper mapper) : IAdminService
    {
        public async Task<Result<IEnumerable<UserForAdminDto>>> GetUserListAsync(CancellationToken ct)
        {
            var dtoList = await userManager.Users
                .Include(u => u.Profile)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ProjectTo<UserForAdminDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return Result<IEnumerable<UserForAdminDto>>.Success(dtoList);
        }
    }
}
