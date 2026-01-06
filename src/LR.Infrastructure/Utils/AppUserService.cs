using AutoMapper;
using AutoMapper.QueryableExtensions;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors;
using LR.Application.AppResult.ResultData;
using LR.Application.DTOs.Admin;
using LR.Application.DTOs.User;
using LR.Application.Interfaces.Utils;
using LR.Application.Requests.Admin;
using LR.Persistance.Extensions;
using LR.Persistance.Identity;
using LR.Persistance.SearchConfig;
using LR.Persistance.SearchConfig.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LR.Infrastructure.Utils
{
    public class AppUserService(
        ILogger<AppUserService> logger,
        UserManager<AppUser> userManager,
        IMapper mapper) 
        : IAppUserService
    {
        public async Task<Result<UserProfileDetailsDto>> GetProfileDetailsAsync(string userId, CancellationToken ct = default)
        {
            try
            {
                var profileDetails = await userManager.Users
                    .Where(u => u.Id == userId)
                    .ProjectTo<UserProfileDetailsDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);

                return profileDetails is null
                    ? Result<UserProfileDetailsDto>.Failure(GeneralErrors.NotFound)
                    : Result<UserProfileDetailsDto>.Success(profileDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed fetch data.");

                return Result<UserProfileDetailsDto>.Failure(GeneralErrors.InternalServerError);
            }

        }

        public async Task<Result<PaginatedResult<UserForAdminDto>>> GetUsersForAdminAsync(UsersForAdminRequest request, string adminId, CancellationToken ct)
        {
            try
            {
                var searchFields = SearchFieldResolver.Resolve(request.SearchIn, UserSearchConfig.Map);

                var paginatedResult = await userManager.Users
                    .Where(u => u.Id != adminId)
                    .ApplyTextSearch(request.Search, searchFields)
                    .ProjectTo<UserForAdminDto>(mapper.ConfigurationProvider)
                    .ApplySorting(request.Sort)
                    .ToPagedAsync(request.PageNumber, request.PageSize, ct);

                return Result<PaginatedResult<UserForAdminDto>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed fetch data.");

                return Result<PaginatedResult<UserForAdminDto>>.Failure(GeneralErrors.InternalServerError);
            }
        }
    }
}
