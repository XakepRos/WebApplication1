using Microsoft.VisualBasic.FileIO;
using WebApplication1.Enum;

namespace WebApplication1.Models
{
    public class FileUploadModel
    {
        public IFormFile FileDetails { get; set; }
        public FileType FileType { get; set; }
    }
}
