using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CustomerRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerRegister(User user, string userName, string Password)
        {
            var Customer = _context.UserLogins.Where(x => x.UserName == userName).SingleOrDefault();
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {

                        user.ImageFile.CopyToAsync(fileStream);
                    }
                    user.ImagePath = imageName;
                    _context.Add(user);
                    _context.SaveChanges();

                    UserLogin usersLogin = new UserLogin();
                    usersLogin.UserName = userName;
                    usersLogin.Password = Password;
                    usersLogin.RoleId = 3;
                    usersLogin.UserId = user.Id;


                    _context.Add(usersLogin);
                    _context.SaveChanges();

                }
            }
            ModelState.AddModelError("", "UserName already exist");
            return RedirectToAction("Login", "LoginAndRegister");
        }

        public IActionResult ChefRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChefRegister(User user, string userName, string Password)
        {
            var Customer = _context.UserLogins.Where(x => x.UserName == userName).SingleOrDefault();
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {

                        user.ImageFile.CopyToAsync(fileStream);
                    }
                    user.ImagePath = imageName;
                    _context.Add(user);
                    _context.SaveChanges();

                    UserLogin usersLogin = new UserLogin();
                    usersLogin.UserName = userName;
                    usersLogin.Password = Password;
                    usersLogin.RoleId = 2;
                    usersLogin.UserId = user.Id;


                    _context.Add(usersLogin);
                    _context.SaveChanges();

                }
            }

            ModelState.AddModelError("", "UserName already exist");
            return RedirectToAction("Login", "LoginAndRegister");

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var user = await _context.UserLogins.
                Where(x => x.UserName == userLogin.UserName && x.Password == userLogin.Password).SingleOrDefaultAsync();

            if (user != null)
            {
                switch (user.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminId", (int)user.UserId);
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        HttpContext.Session.SetInt32("ChefId", (int)user.UserId);
                        return RedirectToAction("Index", "Home");
                    case 3:
                        HttpContext.Session.SetInt32("CustomerId", (int)user.UserId);
                        return RedirectToAction("Index", "Home");

                }
            }
            ModelState.AddModelError("", "UserName or Password are incorret");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
