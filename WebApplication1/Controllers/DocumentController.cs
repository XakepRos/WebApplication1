using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Xml.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DocumentController : Controller
    {

        private readonly ILogger<DocumentController> _logger;
        private readonly DbaseContext _dbContext;
        private readonly IOptions<ApplicationConfigurations> _config;

        public DocumentController(ILogger<DocumentController> logger, DbaseContext dbContext, IOptions<ApplicationConfigurations> config)
        {
            _logger = logger;
            _dbContext = dbContext;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(List<IFormFile> files)
        {
            try
            {
                if(files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string filename = file.FileName;
                        filename = Path.GetFileName(filename);
                        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Documents", filename);

                        var stream = new FileStream(uploadPath, FileMode.Create);
                        file.CopyToAsync(stream);
                    }

                    ViewBag.Message = "Total" + files.Count.ToString() + "File Uploaded Successfully.";
                }
            }
            catch(Exception ex)
            {
                //throw ex;
                ViewBag.Message = "Error while uploading files.";

            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public async Task<ActionResult> UploadDocument(DocumentModel model, IFormFile file)
        {
            
            if (file != null)
            {
                string folder = "NepalLife\\WebApplication1\\WebApplication1\\Document";
                model.DocURL = await UploadImage(folder, file);

            }      
            await _dbContext.AddRangeAsync(model);

            return Ok();
        }

        public async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            var fileType = file.ContentType;
            if (fileType == "image/jpeg" || fileType == "image/png" || fileType == "application/pdf" || fileType == "application/jpg")
            {
                var docPath = _config.Value.DocumentPath;
                //var docPath = Path.Combine("E:\\FileServerDocuments", "\\AgentPortal");
                if (!Directory.Exists(docPath))
                    Directory.CreateDirectory(docPath);

                if (!Directory.Exists(docPath + "\\" + folderPath))
                    Directory.CreateDirectory(docPath + "\\" + folderPath);
                folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

                string serverFolder = Path.Combine(docPath, folderPath);

                await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                return "/" + folderPath;
            }
            return "/" + folderPath;
        }
    }
}
