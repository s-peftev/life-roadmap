using Microsoft.AspNetCore.Http;

namespace LR.Application.Requests.User
{
    public class ProfilePhotoUploadRequest
    {
        public required IFormFile File { get; set; }
    }
}
