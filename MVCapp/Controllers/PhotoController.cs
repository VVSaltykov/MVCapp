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
        public async Task<IActionResult> Upload(IFormFile uploadImage)
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

                Photo photos = new Photo
                {
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
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename is not availble");

            var path = Path.Combine(Directory.GetCurrentDirectory(), _appEnvironment.WebRootPath, "Photos", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        // Get content type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        // Get mime types
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
