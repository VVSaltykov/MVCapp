using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Interfaces;
using MVCapp.Models;
using MVCapp.Repositories;

namespace MVCapp.Controllers
{
    public class HomeController : Controller 
    {
        private readonly BaseRepository<Home> baseRepository;
        
        public HomeController(BaseRepository<Home> baseRepository)
        {
            this.baseRepository = baseRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        [Route("~/Home/Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Home home =new Home();
                byte[] salt = { 1, 2, 3 };

                home.Login = registerModel.Login;
                home.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: registerModel.Password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                await baseRepository.AddToDatabase(home);

                return Redirect("~/");
            }
            return View(registerModel);
        }
    }
}
