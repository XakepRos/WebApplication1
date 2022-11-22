using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DocumentController : Controller
    {

        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
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
    }
}
