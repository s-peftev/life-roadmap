using FluentValidation;
using LR.Application.Requests.User;

namespace LR.Application.Validators.User
{
    public class ProfilePhotoUploadRequestValidator : AbstractValidator<ProfilePhotoUploadRequest>
    {
        private const long MaxFileSize = 4 * 1024 * 1024;

        public ProfilePhotoUploadRequestValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required");

            RuleFor(x => x.File.Length)
                .GreaterThan(0).WithMessage("File is empty")
                .LessThanOrEqualTo(MaxFileSize).WithMessage($"File size must be <= {MaxFileSize / 1024 / 1024} MB");

            RuleFor(x => x.File.ContentType)
                .Must(ct => ct == "image/jpeg" || ct == "image/png")
                .WithMessage("Only JPEG or PNG images are allowed");
        }
    }
}
