using LR.Application.AppResult;
using LR.Application.AppResult.ResultData.Photo;
using Microsoft.AspNetCore.Http;

namespace LR.Application.Interfaces.ExternalProviders
{
    public interface IPhotoService
    {
        Task<Result<PhotoUploadResult>> UploadPhotoAsync(IFormFile file);
        Task<Result> DeletePhotoAsync(string publicId);
    }
}
