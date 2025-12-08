using AutoMapper;
using AutoMapper.QueryableExtensions;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests;
using LR.Domain.Interfaces.Repositories;
using LR.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LR.Infrastructure.Utils
{
    public class AppUserService(
        UserManager<AppUser> userManager,
        IUserProfileRepository userProfileRepository,
        IMapper mapper) 
        : IAppUserService
    {
        public async Task<Result<UserProfileDetailsDto>> GetProfileDetailsAsync(string userId, CancellationToken ct = default)
        { 
            var profileDetails = await userManager.Users
                .Where(u => u.Id == userId)
                .ProjectTo<UserProfileDetailsDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            return profileDetails is null 
                ? Result<UserProfileDetailsDto>.Failure(GeneralErrors.NotFound)
                : Result<UserProfileDetailsDto>.Success(profileDetails);

        }
        public async Task<Result<PaginatedResult<UserForAdminDto>>> GetUsersForAdminAsync(PaginatedRequest request, string adminId, CancellationToken ct)
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
