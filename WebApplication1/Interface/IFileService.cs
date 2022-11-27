using Microsoft.AspNetCore.Mvc;
using WebApplication1.Enum;
using WebApplication1.Models;

namespace WebApplication1.Interface
{
    public interface IFileService
    {
        public Task GetFileAsync([FromForm] FileUploadModel fileDetails, string filePath);
        public Task PostFileAsync(IFormFile fileData, FileType fileType);

        public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task DownloadFileById(int fileName);
    }
}
