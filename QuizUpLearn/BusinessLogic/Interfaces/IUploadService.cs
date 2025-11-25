using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IUploadService
    {
        Task<(string Url, string PublicId)> UploadAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<IFormFile> ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName, string contentType);
    }
}
