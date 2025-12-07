using AutoMapper;
using AutoMapper.QueryableExtensions;
using LR.Application.AppResult;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests;
using LR.Domain.Interfaces.Repositories;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LR.Infrastructure.Utils
{
    public class AdminService(
        UserManager<AppUser> userManager,
        IUserProfileRepository userProfileRepository,
        IMapper mapper) 
        : IAdminService
    {
        public async Task<Result<PaginatedResult<UserForAdminDto>>> GetUserListAsync(PaginatedRequest request, string adminId, CancellationToken ct)
        {
            var q = userManager.Users
                .Include(u => u.Profile)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Id != adminId)
                .ProjectTo<UserForAdminDto>(mapper.ConfigurationProvider);

            var totalCount = await q.CountAsync(ct);
            var profiles = await userProfileRepository.GetAllPaginatedAsync(q, request.PageNumber, request.PageSize, ct);

            var paginatedResult = new PaginatedResult<UserForAdminDto>
            {
                Metadata = new PaginationMetadata
                {
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
                },
                Items = profiles
            };

            return Result<PaginatedResult<UserForAdminDto>>.Success(paginatedResult);
        }
    }
}
