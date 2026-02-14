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
            // model validation check
            if (ModelState.IsValid)
            {

                // check existing user
                var existingUser = _context.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail);

                // send the error message user
                if(existingUser != null)
                {
                    ModelState.AddModelError("UserEmail", "This user already exists");
                    return View(user);
                }

                // save the new user data in database
                _context.Users.Add(user);
                _context.SaveChanges();

                //redirect to login page
                return RedirectToAction("Index", "Login");

            }

            return View(user);
        }

    }
}
