using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LR.Application.AppResult;
using LR.Application.AppResult.Errors.User;
using LR.Application.AppResult.ResultData.Photo;
using LR.Application.Interfaces.ExternalProviders;
using LR.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LR.Infrastructure.ExternalProviders
{
    public class CloudinaryPhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IOptions<CloudinaryOptions> _options;
        public CloudinaryPhotoService(IOptions<CloudinaryOptions> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _options = options;
        }
        public async Task<Result> DeletePhotoAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return Result.Failure(PhotoErrors.PhotoDeletionFailed);

            var deletionParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Error is not null)
                return Result.Failure(PhotoErrors.PhotoDeletionFailed);

            return Result.Success();
        }

        public async Task<Result<PhotoUploadResult>> UploadPhotoAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = _options.Value.LibraryFolder
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error is not null)
                    return Result<PhotoUploadResult>.Failure(PhotoErrors.PhotoUploadFailed);

                return Result<PhotoUploadResult>.Success(new PhotoUploadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.AbsoluteUri
                });
            }

            return Result<PhotoUploadResult>.Failure(PhotoErrors.PhotoUploadFailed);
        }
    }
}
