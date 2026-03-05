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

        // login form create
        public IActionResult Index()
        {
            return View();
        }

        // login form validation
        [HttpPost]
        public IActionResult Index(string UserEmail, string UserPassword)
        {

            // check the user validation
            var user = _context.Users.FirstOrDefault(x => x.UserEmail == UserEmail && x.UserPassword == UserPassword && x.UserStatus == "active");

            if(user != null)
            {
                // set username and roleid in session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetInt32("UserRole", user.RoleId ?? 2);

                // navigate the user check his roleid
                if(user.RoleId == 1)
                {
                    // navigate the admin dashboard home page
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    // navigate the website home page
                    return RedirectToAction("Index", "Customer");
                }

            }

            // user not validate
            ViewBag.ErrorMessage = "Invalid Login Or Account Inactive";
            return View();
        }

        // registation form create
        public IActionResult Registation()
        {
            return View();
        }

        // registaion form validation
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

        // logout-btn
        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult LogOut()
        {
            // resmove the session
            HttpContext.Session.Clear();


            Response.Cookies.Delete(".AspNetCore.Session");


            return RedirectToAction("Index", "Customer");
        }

    }
}
