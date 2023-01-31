using MVCapp.Models;
using MVCapp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MVCapp.Controllers
{
    public class PhotoController : Controller
    {
        private readonly ApplicationContext applicationContext;
        IWebHostEnvironment _appEnvironment;
        private readonly PhotoRepository fileRepository;

        public PhotoController(PhotoRepository fileRepository, ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            this.fileRepository = fileRepository;
            this._appEnvironment = appEnvironment;
            this.applicationContext = context;
        }

        public IActionResult Photo()
        {
            //if (applicationContext.Photos.Any())
            //{
            //    return View(applicationContext.Photos.ToList());
            //}
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                string path = "/Photos/" + uploadImage.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadImage.CopyToAsync(fileStream);
                }
                Photos photos = new Photos { PhotoName = uploadImage.FileName, Path = path };
                await fileRepository.AddPhotoAsync(photos);
            }
            return RedirectToAction("Photo");
        }
    }
}
