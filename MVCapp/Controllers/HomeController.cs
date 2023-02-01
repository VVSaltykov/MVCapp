using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Interfaces;
using MVCapp.Models;
using MVCapp.Repositories;
using System.IO;

namespace MVCapp.Controllers
{
    public class HomeController : Controller 
    {
        ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;

        public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Photos.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Photos file = new Photos { PhotoName = uploadedFile.FileName, Path = path };
                _context.Photos.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
