using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ProductNews.Models;

namespace ProductNews.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductNewsContext context = new();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string username, string pass)
        {
            var admin = context.Accounts.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(pass));
            if(admin != null)
            {
                HttpContext.Session.SetString("admin", admin.ToJson());
                HttpContext.Session.Remove("customer");
            }
            return RedirectToAction("NewsManagement", "News");
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("Index", "Home");
        }
    }
}
