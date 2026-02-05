using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class LoginController : Controller
    {
        // login form
        public IActionResult Index()
        {
            return View();
        }

        //registation form
        public IActionResult Registation()
        {
            return View();
        }

    }
}
