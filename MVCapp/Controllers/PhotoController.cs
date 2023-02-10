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
        private static Random random = new Random();


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
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[16];
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);
                finalString = $"{finalString}.jpeg";

                if (await photoRepository.FileInDataBase(finalString))
                {
                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    finalString = new String(stringChars);
                    finalString = $"{finalString}.jpeg";
                }
                string path = "/Photos/" + finalString;

                var image = Image.FromStream(uploadImage.OpenReadStream());
                var imageBytes = await photoRepository.ResizeImage(image);

                using (var stream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                }

                Photo photos = new Photo
                {
                    PhotoName = uploadImage.FileName,
                    Path = path
                };
                await photoRepository.AddPhotoAsync(photos);
            }
            return RedirectToAction("Photo");
        }
        public async Task<IActionResult> Download(string path)
        {
            var photo = await photoRepository.GetFileAsync(path);
            if (photo.PhotoName == null)
                return Content("filename is not availble");

            var _path = Path.Combine(Directory.GetCurrentDirectory(), _appEnvironment.WebRootPath + path);

            var memory = new MemoryStream();
            using (var stream = new FileStream(_path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(_path), Path.GetFileName(photo.PhotoName));
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
