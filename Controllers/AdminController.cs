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

        // create-banner-form
        public IActionResult CreateBanner()
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

            return View();
        }

        // create-banner-form
        [HttpPost]
        public IActionResult CreateBanner(Banner banner, IFormFile? imageFile)
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

            // banner validation
            if (ModelState.IsValid)
            {
                // image validation check
                if (imageFile != null)
                {
                    // create folder
                    string folder = Path.Combine(_env.WebRootPath, "images", "banner_img");

                    // check folder exists
                    if (!Directory.Exists(folder)) {
                        Directory.CreateDirectory(folder); 
                    }

                    // create file name
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fs);
                    }

                    // save the name in database
                    banner.BannerImage = fileName;


                }

                // save all data
                _context.Banners.Add(banner);
                _context.SaveChanges();

                TempData["Success"] = "Banner Created Successfully!";
                return RedirectToAction("AllBanner", "Admin");

            }

            return View(banner);
        }

        // banner-table
        [HttpGet]
        public IActionResult AllBanner()
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

            // get all banner data
            var allbanner = _context.Banners.ToList();

            // check banner data
            if (allbanner.Count == 0)
            {
                ViewBag.message = "No Banner Data Found!";
            }

            return View(allbanner);
        }

        // edit-banner
        [HttpGet]
        public IActionResult EditBanner(int id)
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

            // banner id validation
            var banner = _context.Banners.FirstOrDefault(b => b.BannerId == id);

            // return error message
            if (banner == null)
            {
                TempData["Error"] = "Banner Not Found";
                return RedirectToAction("AllBanner", "Admin");
            }

            return View(banner);
        }

        // update-banner
        [HttpPost]
        public IActionResult UpdateBanner(Banner banner, IFormFile? imageFile)
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

            // banner validation
            if (ModelState.IsValid)
            {
                // without tracking banner
                var existingBanner = _context.Banners.AsNoTracking().FirstOrDefault(x => x.BannerId == banner.BannerId);

                if (existingBanner == null)
                {
                    TempData["Error"] = "Banner Not Found";
                    return RedirectToAction("AllBanner", "Admin");
                }

                // validation image path
                if (imageFile != null)
                {
                    // save the new image
                    string folder = Path.Combine(_env.WebRootPath, "images", "banner_img");
                    
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // remove the previous image
                    if (!string.IsNullOrEmpty(existingBanner.BannerImage))
                    {
                        string oldFilePath = Path.Combine(folder, existingBanner.BannerImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // save the new image
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(folder, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fs);
                    }
                    // update the model image name
                    banner.BannerImage = fileName;
                }
                else
                {
                    // if image not submit then submit the previous image
                    banner.BannerImage = existingBanner.BannerImage;
                }

                // update the banner
                _context.Banners.Update(banner);
                _context.SaveChanges();

                TempData["Success"] = "Banner Updated Successfully!";
                return RedirectToAction("AllBanner", "Admin");


            }

            return View(banner);
        }

        // delete banner
        [HttpPost]
        public IActionResult DeleteBanner(int id)
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

            // check banner length
            var totalBanners = _context.Banners.Count();

            // send error message
            if (totalBanners <= 3)
            {
                TempData["Error"] = "At Least 3 Banner Must Be Used.";
                return RedirectToAction("AllBanner", "Admin");
            }

            // banner validation
            var bannerToDelete = _context.Banners.FirstOrDefault(b => b.BannerId == id);
            if (bannerToDelete == null)
            {
                TempData["Error"] = "Can Not Find The Banner";
                return RedirectToAction("AllBanner", "Admin");
            }

            // delete image
            if (!string.IsNullOrEmpty(bannerToDelete.BannerImage))
            {
                string folder = Path.Combine(_env.WebRootPath, "images", "banner_img");
                string filePath = Path.Combine(folder, bannerToDelete.BannerImage);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // remove banner
            _context.Banners.Remove(bannerToDelete);
            _context.SaveChanges();

            TempData["Success"] = "Delete Successfully";
            return RedirectToAction("AllBanner", "Admin");
        }


        // contact-table
        public IActionResult Contact()
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

            // get all contact data
            var allcontact = _context.Contacts.OrderByDescending(x => x.CreatedAt).ToList();

            // check contact data
            if (allcontact.Count == 0)
            {
                ViewBag.message = "No Contact Data Found!";
            }

            return View(allcontact);
        }

        // delete-contact
        [HttpPost]
        public IActionResult DeleteContact(int id)
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


            // delete the contact
            var data = _context.Contacts.Find(id);
            if (data != null)
            {
                _context.Contacts.Remove(data);
                _context.SaveChanges();
                TempData["Success"] = "Contact Deleted successfully!";
            }

            return RedirectToAction("Contact", "Admin");
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

        // edit-gender-form
        [HttpGet]
        public IActionResult EditGender(int id)
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

            // check gender id
            var genderId = _context.Genders.Find(id);

            if(genderId == null)
            {
                TempData["Error"] = "Gender Can Not Find!";
                return RedirectToAction("AllGender", "Admin");
            }

            return View(genderId);
        }

        // update-gender
        [HttpPost]
        public IActionResult UpdateGender(Gender gender)
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

            // update data in database
            if (ModelState.IsValid)
            {
                _context.Genders.Update(gender);
                _context.SaveChanges();
                TempData["Success"] = "Gender updated successfully!";
                return RedirectToAction("AllGender", "Admin");
            }

            return View(gender);

        }

        // delete-gender
        [HttpPost]
        public IActionResult DeleteGender(int id)
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

            // user check if gender use
            var hasUsers = _context.Users.Any(x => x.GenderId == id);

            if (hasUsers)
            {
                TempData["Error"] = "This gender cannot be deleted!";
                return RedirectToAction("AllGender", "Admin");
            }

            // delete the gender
            var data = _context.Genders.Find(id);
            if (data != null)
            {
                _context.Genders.Remove(data);
                _context.SaveChanges();
                TempData["Success"] = "Gender deleted successfully!";
            }

            return RedirectToAction("AllGender", "Admin");
        }

        // create-city-form
        public IActionResult CreateCity()
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

            return View();
        }

        // create-city-form
        [HttpPost]
        public IActionResult CreateCity(City city)
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

            // city data check
            var isExists = _context.Cities.Any(x => x.CityName.ToLower() == city.CityName.ToLower().Trim());

            if (isExists)
            {
                ModelState.AddModelError("CityName", "This City Already Exists");
                return View(city);
            }

            // insert data in database
            if (ModelState.IsValid)
            {
                _context.Cities.Add(city);
                _context.SaveChanges();
                return RedirectToAction("AllCity", "Admin");
            }

            return View(city);
        }

        // all-city-table
        [HttpGet]
        public IActionResult AllCity()
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

            // get all city data
            var allcity = _context.Cities.Include(x => x.Users).ToList();

            // check gender data
            if (allcity.Count == 0)
            {
                ViewBag.message = "No City Data Found!";
            }

            return View(allcity);
        }

        // edit-city-form
        [HttpGet]
        public IActionResult EditCity(int id)
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

            // check city id
            var cityId = _context.Cities.Find(id);

            if (cityId == null)
            {
                TempData["Error"] = "City Can Not Find!";
                return RedirectToAction("AllCity", "Admin");
            }
            return View(cityId);
        }

        // update-city
        public IActionResult UpdateCity(City city)
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

            // validation check
            if (ModelState.IsValid)
            {
                _context.Cities.Update(city);
                _context.SaveChanges();
                TempData["Success"] = "City updated successfully!";
                return RedirectToAction("AllCity", "Admin");
            }
            return View(city);

        }

        // delete-city
        [HttpPost]
        public IActionResult DeleteCity(int id)
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

            // user check if city use
            var hasUsers = _context.Users.Any(x => x.CityId == id);

            if (hasUsers)
            {
                TempData["Error"] = "This city cannot be deleted!";
                return RedirectToAction("AllCity", "Admin");
            }

            // delete the city
            var data = _context.Cities.Find(id);
            if (data != null)
            {
                _context.Cities.Remove(data);
                _context.SaveChanges();
                TempData["Success"] = "City deleted successfully!";
            }

            return RedirectToAction("AllCity", "Admin");
        }

        // create-country-form
        public IActionResult CreateCountry()
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

            return View();
        }

        // create-country-form
        [HttpPost]
        public IActionResult CreateCountry(Country country)
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

            // country data check
            var isExists = _context.Countries.Any(x => x.CountryName.ToLower() == country.CountryName.ToLower().Trim());

            if (isExists)
            {
                ModelState.AddModelError("CountryName", "This Country Already Exists");
                return View(country);
            }

            // insert data in database
            if (ModelState.IsValid)
            {
                _context.Countries.Add(country);
                _context.SaveChanges();
                return RedirectToAction("AllCountry", "Admin");
            }

            return View(country);
        }

        // all-country-table
        [HttpGet]
        public IActionResult AllCountry()
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

            // get all country data
            var allcountry = _context.Countries.Include(x => x.Users).ToList();

            // check country data
            if (allcountry.Count == 0)
            {
                ViewBag.message = "No Country Data Found!";
            }

            return View(allcountry);
        }

        // edit-country-form
        [HttpGet]
        public IActionResult EditCountry(int id)
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

            // check country id
            var countryId = _context.Countries.Find(id);

            if(countryId == null)
            {
                TempData["Error"] = "Country Can Not Find!";
                return RedirectToAction("AllCountry", "Admin");
            }

            return View(countryId);
        }

        // update-country
        [HttpPost]
        public IActionResult UpdateCountry(Country country)
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

            // validation check
            if (ModelState.IsValid)
            {
                _context.Countries.Update(country);
                _context.SaveChanges();
                TempData["Success"] = "Country updated successfully!";
                return RedirectToAction("AllCountry", "Admin");
            }
            return View(country);

        }

        // delete-country
        [HttpPost]
        public IActionResult DeleteCountry(int id)
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

            // user check if country use
            var hasUsers = _context.Users.Any(x => x.CountryId == id);

            if (hasUsers)
            {
                TempData["Error"] = "This country cannot be deleted!";
                return RedirectToAction("AllCountry", "Admin");
            }

            // delete the country
            var data = _context.Countries.Find(id);
            if (data != null)
            {
                _context.Countries.Remove(data);
                _context.SaveChanges();
                TempData["Success"] = "Country deleted successfully!";
            }

            return RedirectToAction("AllCountry", "Admin");
        }


        // create-role-form
        public IActionResult CreateRole()
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

            return View();
        }

        // create-role-form
        [HttpPost]
        public IActionResult CreateRole(Role role)
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

            // role data check
            var isExists = _context.Roles.Any(x => x.RoleName.ToLower() == role.RoleName.ToLower().Trim());

            if (isExists)
            {
                ModelState.AddModelError("RoleName", "This Role Already Exists");
                return View(role);
            }

            // insert data in database
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return RedirectToAction("AllRole", "Admin");
            }

            return View(role);
        }

        // all-role-table
        [HttpGet]
        public IActionResult AllRole()
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

            // get all role data
            var allrole = _context.Roles.Include(x => x.Users).ToList();

            // check role data
            if (allrole.Count == 0)
            {
                ViewBag.message = "No Role Data Found!";
            }

            return View(allrole);
        }

        // delete-role
        [HttpPost]
        public IActionResult DeleteRole(int id)
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

            // user check if role use
            var hasUsers = _context.Users.Any(x => x.RoleId == id);

            if (hasUsers)
            {
                TempData["Error"] = "This role cannot be deleted!";
                return RedirectToAction("AllRole", "Admin");
            }

            // delete the role
            var data = _context.Roles.Find(id);
            if (data != null)
            {
                _context.Roles.Remove(data);
                _context.SaveChanges();
                TempData["Success"] = "Role deleted successfully!";
            }

            return RedirectToAction("AllRole", "Admin");
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
