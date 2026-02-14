using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class LoginController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LoginController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // login form
        public IActionResult Index()
        {
            return View();
        }

        // registation form
        public IActionResult Registation()
        {
            return View();
        }

        // registaion create
        [HttpPost]
        public IActionResult Registation(User user)
        {

            return View();
        }

    }
}
