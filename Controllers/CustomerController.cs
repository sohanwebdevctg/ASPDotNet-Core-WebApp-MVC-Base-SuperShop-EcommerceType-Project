using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Models;
using SuperShop.ViewModels;

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

        // show banner, category, product, offer
        public IActionResult Index()
        {
            var viewModel = new HomeVM
            {
                // Only categories that have at least 1 product
                Categories = _context.Categoreis
                             .Where(c => _context.Products.Any(p => p.CategoryId == c.CategoryId))
                             .ToList(),

                // Latest 8 products
                Products = _context.Products.OrderByDescending(p => p.ProductId).Take(8).ToList(),

                // 3 banners (1 main, 2 side)
                Banners = _context.Banners.Take(3).ToList(),

                // 3 offer cards
                Offers = _context.Offers.Take(3).ToList()
            };

            return View(viewModel);
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

        // update-user
        [HttpGet]
        public IActionResult UpdateUser(int id)
        {

            // get user data in session
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            var sessionUserRole = HttpContext.Session.GetInt32("UserRole");

            // validation check session data
            if (sessionUserId == null || sessionUserRole != 2 || sessionUserId != id)
            {
                return RedirectToAction("Index", "Login");
            }

            // database check
            var dbUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // show viewmodel
            var viewModel = new UserEditVM
            {
                UserData = dbUser,
                GenderList = _context.Genders.Select(g => new SelectListItem { Value = g.GenderId.ToString(), Text = g.GenderName }),
                CityList = _context.Cities.Select(g => new SelectListItem { Value = g.CityId.ToString(), Text = g.CityName }),
                CountryList = _context.Countries.Select(g => new SelectListItem { Value = g.CountryId.ToString(), Text = g.CountryName }),
                RoleList = _context.Roles.Select(g => new SelectListItem { Value = g.RoleId.ToString(), Text = g.RoleName })

            };

            return View(viewModel);
        }

        // update-user
        [HttpPost]
        public IActionResult UpdateUser(UserEditVM userModel, IFormFile? imageFile)
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
            var dbUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == sessionUserId);

            //validation check database data
            if (dbUser == null || dbUser.UserStatus != "active" || dbUser.RoleId != sessionUserRole)
            {
                // remove session data
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("UserRole");

                // redirect to the user login page
                return RedirectToAction("Index", "Login");
            }

            // check user id validation
            if (userModel.UserData.UserId != sessionUserId)
            {
                TempData["Error"] = "Unauthorized Access!";
                return RedirectToAction("UserProfile", "Customer");
            }

            // user data validation
            if (ModelState.IsValid)
            {

                // check user validation
                var existingUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.UserId == userModel.UserData.UserId);

                if (existingUser == null)
                {
                    TempData["Error"] = "User Not Found";
                    return RedirectToAction("UserProfile", "Customer");
                }

                // create folder path
                string folder = Path.Combine(_env.WebRootPath, "images", "user_img");

                // delete previous image
                if (imageFile != null)
                {
                    
                    if (!string.IsNullOrEmpty(existingUser.UserImage) && existingUser.UserImage != "no-user.png")
                    {
                        string oldFilePath = Path.Combine(folder, existingUser.UserImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // save new image
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fs);
                    }
                    userModel.UserData.UserImage = fileName;
                }
                else
                {
                    // set previous image
                    userModel.UserData.UserImage = existingUser.UserImage;
                }

                
                userModel.UserData.UserStatus = existingUser.UserStatus;
                userModel.UserData.RoleId = existingUser.RoleId;

                // update database
                _context.Users.Update(userModel.UserData);
                _context.SaveChanges();

                return RedirectToAction("UserProfile", "Customer");

            }

            // return viewmodel
            userModel.GenderList = _context.Genders.Select(g => new SelectListItem { Value = g.GenderId.ToString(), Text = g.GenderName });
            userModel.CityList = _context.Cities.Select(c => new SelectListItem { Value = c.CityId.ToString(), Text = c.CityName });
            userModel.CountryList = _context.Countries.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.CountryName });
            userModel.RoleList = _context.Roles.Select(r => new SelectListItem { Value = r.RoleId.ToString(), Text = r.RoleName });


            return View(userModel);
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
