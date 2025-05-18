using Cafe.BLL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly CafeFacade _cafeFacade;

        public AccountController(CafeFacade cafeFacade)
        {
            _cafeFacade = cafeFacade;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _cafeFacade.LoginAsync(username, password);
            if (user == null)
            {
                ViewBag.Error = "Невірний логін або пароль.";
                return View();
            }

            HttpContext.Session.SetString("User", user.Username);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var success = await _cafeFacade.RegisterAsync(username, password);
            if (!success)
            {
                ViewBag.Error = "Користувач із таким іменем уже існує.";
                return View();
            }

            HttpContext.Session.SetString("User", username);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }
    }
}