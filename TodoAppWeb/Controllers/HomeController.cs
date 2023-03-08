using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoAppWeb.Models.ViewModels;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode = 404)
        {
            return View(new ErrorViewModel() { ErrorCode = StatusCode(statusCode) });
        }

        [Route("Login/")]
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Account");
        }

        [Route("Register/")]
        public IActionResult Register()
        {
            return RedirectToAction("Register", "Account");
        }

        [Route("Logout/")]
        public IActionResult Logout()
        {
            return RedirectToAction("Logout", "Account");
        }
    }
}