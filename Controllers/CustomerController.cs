using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CustomerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            return View();
        }

        // userporfile
        public IActionResult UserProfile()
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users
                .Include(r => r.Role)
                .Include(g => g.Gender)
                .Include(c => c.City)
                .Include(c => c.Country)
                .FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            return View(dbUser);
        }

        //allproducts
        public IActionResult AllProducts()
        {
            return View();
        }

        //productdetails
        public IActionResult ProductDetails()
        {
            return View();
        }

        //about
        public IActionResult About()
        {
            return View();
        }

        // contact-form
        public IActionResult Contact()
        {
            // user id check from session
            var userId = HttpContext.Session.GetInt32("UserId");

            // validation the user id
            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

                // take userName & userEmail
                if(user != null)
                {
                    ViewBag.userName = user.UserName;
                    ViewBag.userEmail = user.UserEmail;
                }
            }

            return View();
        }

        // contact-form-submit
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            // model validation check
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                TempData["Success"] = "Your Data Submit Successfully!";
                return RedirectToAction("Contact", "Customer");
            }

            // error message send
            TempData["Error"] = "Please Fill Up The Full Data";

            // return the user data
            ViewBag.userName = contact.UserName;
            ViewBag.userEmail = contact.UserEmail;

            return View(contact);
        }

        // blog
        public IActionResult Blog()
        {
            return View();
        }

        // order-table
        public IActionResult Order()
        {
           return View();
        }
    }
}
