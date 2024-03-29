﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCapp.Interfaces;
using MVCapp.Models;
using MVCapp.Repositories;
using ProMVC.Repositories;
using System.Security.Claims;
using File = MVCapp.Models.File;

namespace MVCapp.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationContext applicationContext;
        IWebHostEnvironment _appEnvironment;
        private readonly FileRepository fileRepository;
        private readonly EventLogRepository eventLogRepository;
        private readonly IUser _IUser;


        public FileController(FileRepository fileRepository, ApplicationContext context, IWebHostEnvironment appEnvironment, EventLogRepository eventLogRepository, IUser _IUser)
        {
            this.fileRepository = fileRepository;
            _appEnvironment = appEnvironment;
            applicationContext = context;
            this.eventLogRepository = eventLogRepository;
            this._IUser = _IUser;
        }

        [HttpGet]
        public IActionResult File()
        {
            var user = applicationContext.Users.FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);
			var tuple = Tuple.Create<List<File>, User>(applicationContext.Files.ToList(), user);
            return View(tuple);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadFile)
        {
            if (uploadFile != null)
            {
                string login = HttpContext.User.Identity.Name;
                var user = await _IUser.GetUserByLoginAsync(login);
                await fileRepository.Upload(uploadFile);
                await eventLogRepository.AddLogger("Пользователь добавил файл", user);
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
        public async Task<IActionResult> Delete(string path)
        {
            var file = await fileRepository.GetFileAsync(path);
            file.DeleteStatus = 1;
            await applicationContext.SaveChangesAsync();
            return RedirectToAction("File");
        }
        public async Task<IActionResult> Recover(string path)
        {
			var file = await fileRepository.GetFileAsync(path);
			file.DeleteStatus = 0;
			await applicationContext.SaveChangesAsync();
			return RedirectToAction("File");
		}
    }
}
