using Microsoft.AspNetCore.Mvc;
using MVCapp.Repositories;
using System.Drawing;
using System.Text.RegularExpressions;
using File = MVCapp.Models.File;

namespace MVCapp.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationContext applicationContext;
        IWebHostEnvironment _appEnvironment;
        private readonly FileRepository fileRepository;
        private static Random random = new Random();


        public FileController(FileRepository fileRepository, ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            this.fileRepository = fileRepository;
            _appEnvironment = appEnvironment;
            applicationContext = context;
        }

        [HttpGet]
        public IActionResult File()
        {
            return View(applicationContext.Files.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadFile)
        {
            if (uploadFile != null)
            {
                var type = fileRepository.GetContentType(uploadFile.FileName);
                var key = fileRepository.GetContentKey(uploadFile.FileName);
                if (Regex.IsMatch(type, @"image(\w*)"))
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var stringChars = new char[16];
                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    var finalString = new String(stringChars);
                    finalString = string.Concat(finalString, key);

                    if (!await fileRepository.FileInDataBase(finalString))
                    {
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        finalString = new String(stringChars);
                        finalString = string.Concat(finalString, key);
                    }
                    string path = "/Files/" + finalString;

                    var image = Image.FromStream(uploadFile.OpenReadStream());
                    var imageBytes = await fileRepository.ResizeImage(image);
                    using (var stream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                    {
                        stream.Write(imageBytes, 0, imageBytes.Length);
                    }

                    File files = new File
                    {
                        FileName = uploadFile.FileName,
                        Path = path
                    };
                    await fileRepository.AddFileAsync(files);
                }
                else
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var stringChars = new char[16];
                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    var finalString = new String(stringChars);
                    finalString = string.Concat(finalString, key);

                    if (!await fileRepository.FileInDataBase(finalString))
                    {
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        finalString = new String(stringChars);
                        finalString = string.Concat(finalString, key);
                    }
                    string path = "/Files/" + finalString;

                    using var imageStream = new MemoryStream();
                    var imageBytes = imageStream.ToArray();

                    using (var stream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create, FileAccess.Write, FileShare.Write, 4096))
                    {
                        stream.Write(imageBytes, 0, imageBytes.Length);
                    }

                    File files = new File
                    {
                        FileName = uploadFile.FileName,
                        Path = path
                    };
                    await fileRepository.AddFileAsync(files);
                }
            }
            return RedirectToAction("File");
        }
        public async Task<IActionResult> Download(string path)
        {
            var file = await fileRepository.GetFileAsync(path);
            if (file.FileName == null)
                return Content("filename is not availble");

            var _path = Path.Combine(Directory.GetCurrentDirectory(), _appEnvironment.WebRootPath + path);

            var memory = new MemoryStream();
            using (var stream = new FileStream(_path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, fileRepository.GetContentType(_path), Path.GetFileName(file.FileName));
        }
    }
}
