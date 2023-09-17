using Microsoft.AspNetCore.Mvc;

namespace FileWeb.Controllers
{
    public class DownloadController : Controller
    {
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(ILogger<DownloadController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public IActionResult GetImage(string image)
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var fullPath = Path.Combine(rootPath, "fileManagement", image);


            FileStream fs = new FileStream(fullPath, FileMode.Open);


            if (fs == null) return NotFound("Couldn't find requested Image");

            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Close();

            return File(ms.ToArray(), "image/png");
        }





        public IActionResult Index()
        {
            _logger.LogInformation("DownloadController: Index() called");
            return View();
        }


        public IActionResult Download(string fileName)
        {
            /*
             * write code here that takes the fileName, 
             * reads it from a directory outside webroot like appData using env into a FileStream/input FileStream/FileStreamResult,
             * and returns the image from the image from the FileStreamResult as HttpResponse/HttpResponseMessage with content type as Image/png
            */



            return View(fileName);
        }
    }
}
