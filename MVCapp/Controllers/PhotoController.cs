using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Models;
using MVCapp.Repositories;
using System.Drawing;
using System.Drawing.Imaging;

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

                var image = Image.FromStream(uploadImage.OpenReadStream());
                var resized = new Bitmap(image, new Size(1920, 1080));
                using var imageStream = new MemoryStream();
                resized.Save(imageStream, ImageFormat.Jpeg);
                var imageBytes = imageStream.ToArray();

                using (var stream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                }
                byte[] salt = { 1, 2, 3 };

                Photos photos = new Photos { 
                    PhotoName = uploadImage.FileName,
                    Path = path,
                    SecondName = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: uploadImage.FileName!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8))
                    };
                await photoRepository.AddPhotoAsync(photos);
            }
            return RedirectToAction("Photo");
        }
    }
}
