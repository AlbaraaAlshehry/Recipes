using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using Recipes.Models;

namespace Recipes.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AdminProfile()// not working
        {
            var AdminId = HttpContext.Session.GetInt32("AdminId");

            var AdminProfile = _context.UserLogins.Include(u => u.User).Where(x => x.UserId == AdminId).FirstOrDefault();
            return View(AdminProfile);
        }
        public IActionResult Contact()
        {
            var contact = _context.Contacts.ToList();
            return View(contact);
        }
        public IActionResult EditAdminProfile(UserLogin userLogin)
        {
            if (userLogin.User.ImageFile != null)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + userLogin.User.ImageFile.FileName;
                string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {

                    userLogin.User.ImageFile.CopyToAsync(fileStream);
                }
                userLogin.User.ImagePath = imageName;

            }
            else
            {
                var image = _context.Users.AsNoTracking().Where(x => x.Id == userLogin.User.Id).FirstOrDefault();
                userLogin.User.ImagePath = image.ImagePath;
            }


            _context.Update(userLogin);
            _context.SaveChanges();


            return RedirectToAction("AdminProfile", "Admin");
        }
        public IActionResult Index() //category done , 
        {
            var x = new List<string> { "Chef", "Customer ", "Recipe" };
         
            var ChefCount = _context.UserLogins.Where(x => x.RoleId==2).Include(x=>x.User).Count();
            var CustomerCount = _context.UserLogins.Where(x => x.RoleId == 2).Include(x => x.User).Count();
            var RecipeCount = _context.Recipes.Count();
            var y = new List<int> { ChefCount, CustomerCount, RecipeCount };
            var chart = Tuple.Create<List<string>, List<int>>(x,y );
            ViewBag.categories = _context.Categories.ToList();
            ViewBag.allUsers = _context.Users.Include(x=>x.UserLogins).ToList();
            ViewBag.Admins = _context.UserLogins.Where(x=>x.RoleId==1).Include(x=>x.User).ToList();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalRecipes= _context.Recipes.Count();
            ViewBag.TotalChefs = _context.UserLogins.Include(x=>x.User).Where(x=>x.RoleId == 2).Count();
            var id = HttpContext.Session.GetInt32("AdminId");
            return View(chart);
        }
        public IActionResult AllRecipes()
        {
            var recipes = _context.Recipes.Include(x => x.User).ToList();
            return View(recipes);
        }
        public IActionResult allUsers() 
        {
            var allUsers= _context.UserLogins.Include(x=>x.User).Include(x=>x.Role).ToList();
            return View(allUsers);
        }
        public IActionResult Reports()
        {
            var chefRecipes = _context.UserRecipes.Include(x=>x.User).Include(x=>x.Recipe).ToList();
            return View(chefRecipes);
        }
        [HttpPost]
        public IActionResult Reports(DateTime? startDate, DateTime? endDate)
        {
            var result = _context.UserRecipes.Include(x => x.Recipe).Include(x => x.User).ToList();

            if (startDate == null && endDate == null)
            {
                ViewBag.TotalPrice = result.Sum(x => x.Recipe.Price );
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {

                result = result.Where(x => x.ParchaseDate.Value.Date >= startDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Recipe.Price);

                return View(result);
            }
            else if (startDate == null && endDate != null)
            {

                result = result.Where(x => x.ParchaseDate.Value.Date <= endDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Recipe.Price);

                return View(result);
            }
            else
            {

                result = result.Where(x => x.ParchaseDate.Value.Date >= startDate && x.ParchaseDate.Value.Date <= endDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Recipe.Price);
                return View(result);
            }
        }
        public IActionResult Search(DateTime? startDate , DateTime? endDate , decimal Id , string status)
        {
            var result = _context.Recipes.Include(x => x.User).Include(x=>x.Category).ToList();
            var request = _context.Recipes.Find(Id);

            if (request != null)
            {
                request.Status = status;
                _context.SaveChanges();
            }

            if (startDate == null && endDate == null)
            {
                ViewBag.TotalPrice = result.Sum(x => x.Price);
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {

                result = result.Where(x => x.CreatedDate.Value.Date >= startDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Price);

                return View(result);
            }
            else if (startDate == null && endDate != null)
            {

                result = result.Where(x => x.CreatedDate.Value.Date <= endDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Price);

                return View(result);
            }
            else
            {

                result = result.Where(x => x.CreatedDate.Value.Date >= startDate && x.CreatedDate.Value.Date <= endDate).ToList();
                ViewBag.TotalPrice = result.Sum(x => x.Price);
                return View(result);
            }
            
        }
        public IActionResult Testimonials(decimal id , string status)
        {
            var request = _context.Tetstimonials.Find(id);
            var testimonial = _context.Tetstimonials.Include(x=>x.User).ToList();
            if (request != null)
            {
                request.Status = status;
                _context.SaveChanges();
            }
            return View(testimonial);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(" ", " ");
        }
    }
}
