using MVCapp.Models;
using MVCapp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MVCapp.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationContext applicationContext;
        IWebHostEnvironment _appEnvironment;
        private readonly PhotoRepository photoRepository;

        public PhotoController(PhotoRepository photoRepository, ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            this.photoRepository = photoRepository;
            this._appEnvironment = appEnvironment;
            this.applicationContext = context;
        }

        [HttpGet]
        public IActionResult Photo()
        {
            return View(applicationContext.Photos.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(IFormFile uploadImage)
        {
            if (uploadImage != null)
            {
                string path = "/Photos/" + uploadImage.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadImage.CopyToAsync(fileStream);
                }
                Photos photos = new Photos { PhotoName = uploadImage.FileName, Path = path };
                await photoRepository.AddPhotoAsync(photos);
            }
            return RedirectToAction("Photo");
        }
    }
}
