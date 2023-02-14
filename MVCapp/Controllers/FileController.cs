using Microsoft.AspNetCore.Mvc;
using MVCapp.Repositories;

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
                await fileRepository.Upload(uploadFile);
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
