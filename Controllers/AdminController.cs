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

        // user profile
        public IActionResult UserProfile()
        {
            return View();
        }

        // create-gender
        public IActionResult CreateGender()
        {
            return View();
        }

        // all-gender-table
        public IActionResult AllGender()
        {
            return View();
        }
    }
}
