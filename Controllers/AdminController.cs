using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class AdminController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // all users
        public IActionResult Index()
        {
            return View();
        }

        // update user
        public IActionResult UpdateUser()
        {
            return View();
        }

        // faq-table
        public IActionResult Faq()
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

        // create-gender-form
        public IActionResult CreateGender()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if(sessionUserId == null || sessionUserRole != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // create-gender-form
        [HttpPost]
        public IActionResult CreateGender(Gender gender)
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // gender data check
            var isExists = _context.Genders.Any(x => x.GenderName.ToLower() == gender.GenderName.ToLower().Trim());

            if (isExists)
            {
                ModelState.AddModelError("GenderName", "This Gender Already Exists");
                return View(gender);
            }

            // insert data in database
            if (ModelState.IsValid)
            {
                _context.Genders.Add(gender);
                _context.SaveChanges();
                return RedirectToAction("AllGender", "Admin");
            }

            return View(gender);
        }



        // all-gender-table
        [HttpGet]
        public IActionResult AllGender()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 1)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // get all gender data
            var allgender = _context.Genders.Include(x => x.Users).ToList();

            // check gender data
            if(allgender.Count == 0)
            {
                ViewBag.message = "No Gender Data Found!";
            }

            return View(allgender);
        }

        // create-city
        public IActionResult CreateCity()
        {
            return View();
        }

        // all-city-table
        public IActionResult AllCity()
        {
            return View();
        }

        // create-country
        public IActionResult CreateCountry()
        {
            return View();
        }

        // all-country-table
        public IActionResult AllCountry()
        {
            return View();
        }

        // create-role
        public IActionResult CreateRole()
        {
            return View();
        }

        // all-role-table
        public IActionResult AllRole()
        {
            return View();
        }

        // create-offer
        public IActionResult CreateOffer()
        {
            return View();
        }

        // all-offer-table
        public IActionResult AllOffer()
        {
            return View();
        }

        // create-category
        public IActionResult CreateCategory()
        {
            return View();
        }

        // all-category-table
        public IActionResult AllCategory()
        {
            return View();
        }

        // create-product
        public IActionResult CreateProduct()
        {
            return View();
        }

        // all-product-table
        public IActionResult AllProduct()
        {
            return View();
        }

    }
}
