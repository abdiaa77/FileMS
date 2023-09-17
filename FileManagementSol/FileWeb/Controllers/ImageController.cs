using FileWeb.DTO.Image;
using FileWeb.DTO.ViewModels;
using FileWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileWeb.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IFileService _fileService;
        public ImageController(ILogger<ImageController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }





        [HttpGet]
        public async Task<ActionResult<ImageRes>> Index()
        {
            _logger.LogInformation("Get all images request");
            var images = await _fileService.GetAllImages();

            _logger.LogInformation("Get All Images response");
            return View(images);
        }


        [HttpGet]
        public async Task<ActionResult<ImageRes>> GetImageById(int id)
        {
            _logger.LogInformation("Get Image by Id Request");
            var image = await _fileService.GetImageById(id);
            _logger.LogInformation("Get Image By Id Response");
            return View(image);
        }



        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<ReponseVM>> UploadImage(IFormFile file)
        {
            var result = await _fileService.UploadImage(file);

            TempData[result.Title] = result.Message;

            if (result.Success == true)
                return RedirectToAction("Index", "Image");

            return View();
        }





    }
}
