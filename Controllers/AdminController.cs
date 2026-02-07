using Microsoft.AspNetCore.Mvc;

namespace SuperShop.Controllers
{
    public class AdminController : Controller
    {
        // userdata
        public IActionResult Index()
        {
            return View();
        }

        // admin profile
        public IActionResult AdminProfile()
        {
            return View();
        }
    }
}
