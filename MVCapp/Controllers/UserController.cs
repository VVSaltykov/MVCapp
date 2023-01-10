using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Models;
using System.Security.Claims;
using MVCapp.Interfaces;
using MVCapp.Exceptions;

namespace MVCapp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _IUser;
        public UserController(IUser _IUser)
        {
            this._IUser = _IUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("~/User/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Route("~/User/Register")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (await _IUser.UserIsInDatabase(registerModel))
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                else
                {
                    User user = new User { Login = registerModel.Login, Password = registerModel.Password };
                    await _IUser.AddNewUser(user);

                    //await Authenticate(user);
                    //HttpContext.Response.Cookies.Append("id", user.Id.ToString());

                    return Redirect("~/");
                }
            }
            return View(registerModel);
        }

        [Route("~/User/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("~/User/Login/{ReturnUrl?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _IUser.GetUserByLoginModelAsync(loginModel);
                    await Authenticate(user);
                    HttpContext.Response.Cookies.Append("id", user.Id.ToString());

                    return Redirect(ReturnUrl ?? "~/");
                }
                catch (NotFoundException)
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(loginModel);
        }

        /*[Route("~/User/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete("id");
            return Redirect("~/Index");
        }*/

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
