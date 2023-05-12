using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OnlineStoreForWoman.Services;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadImage(IFormFile formFile, bool profile);
    Task<DeletionResult> DeleteImage(string publicId);
}