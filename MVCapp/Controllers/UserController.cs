using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Exceptions;
using MVCapp.Interfaces;
using MVCapp.Models;
using MVCapp.Repositories;
using ProMVC.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MVCapp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _IUser;
        public UserController(IUser _IUser)
        {
            this._IUser = _IUser;
        }

        [Route("~/User/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Route("~/User/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User user = new();
                if (await _IUser.UserIsInDatabase(registerModel))
                {
                    ModelState.AddModelError("", "Такой пользователь уже существует!");
                }
                else
                {
                    byte[] salt = { 1, 2, 3 };

                    user.Login = registerModel.Login;
                    user.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: registerModel.Password!,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));

                    await _IUser.AddNewUser(user);
                    
                    await Authenticate(user);
                    HttpContext.Response.Cookies.Append("id", user.Id.ToString());

                    return Redirect("~/");
                }
            }
            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("~/User/Login/{ReturnUrl?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _IUser.GetUserByLoginModelAsync(loginModel);
                    if (user != null)
                    {
                        await Authenticate(user);
                        HttpContext.Response.Cookies.Append("id", user.Id.ToString());

                        return Redirect("~/Photo/Photo");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    }
                }
                catch (NotFoundException)
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(loginModel);
        }

        [Route("~/User/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete("id");
            return Redirect("~/User/Login");
        }

        private async Task Authenticate(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
