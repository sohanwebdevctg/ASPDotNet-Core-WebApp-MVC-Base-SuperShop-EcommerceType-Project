using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //login
        public IActionResult Login()
        {
            return View();
        }

        //registation
        public IActionResult Registation()
        {
            return View();
        }

    }
}
