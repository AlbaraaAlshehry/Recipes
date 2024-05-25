using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.EntityFrameworkCore;
using Recipes.Models;
using System.Diagnostics;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;



namespace Recipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
       

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            
        }

        public IActionResult ChefProfile()
        {
            var ChefId = HttpContext.Session.GetInt32("ChefId");

            var ChefProfile = _context.UserLogins.Include(u => u.User).Where(x => x.UserId == ChefId).FirstOrDefault();
            return View(ChefProfile);
        }
        public IActionResult CustomerProfile()
        {
            var CustomerId = HttpContext.Session.GetInt32("CustomerId");

            var CustomerProfile = _context.UserLogins.Include(u => u.User).Where(x => x.UserId == CustomerId).FirstOrDefault();
            return View(CustomerProfile);
        }
        public IActionResult UserTestimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserTestimonial(Tetstimonial tetstimonial , string TestimonialText)//not working
        {
            var testimonial = _context.Tetstimonials.Where(x=>x.TestimonialText == TestimonialText).FirstOrDefault();
            if (testimonial == null)
            {
                Tetstimonial newTestimonial = new Tetstimonial();
                newTestimonial.TestimonialText = TestimonialText;
                newTestimonial.CreatedDate = DateTime.Now;
                _context.Add(newTestimonial);
                _context.SaveChanges();
            }

            return View();
        }
        [HttpPost]
        public IActionResult EditChefProfile(UserLogin userLogin)
        {
            if (userLogin.User.ImageFile!=null)
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
            var image = _context.Users.AsNoTracking().Where(x=>x.Id== userLogin.User.Id).FirstOrDefault();
                userLogin.User.ImagePath = image.ImagePath;
            }

         
            _context.Update(userLogin);
            _context.SaveChanges(); 

           
            return RedirectToAction("ChefProfile", "Home");
        }
        [HttpPost]
        public IActionResult EditCustomerProfile(UserLogin userLogin)
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


            return RedirectToAction("CustomerProfile", "Home");
        }
        
        public IActionResult Index()
        {
            ViewBag.heroParagraph=_context.Homes.FirstOrDefault();
            ViewBag.heroImage=_context.Homes.FirstOrDefault();
            ViewBag.About=_context.Homes.FirstOrDefault();
            var Categories = _context.Categories.ToList();
            var tetstimonials = _context.Tetstimonials.Include(x=>x.User).Where(x => x.Status == "Accepted").ToList();
            var receipes = _context.Recipes.ToList();
            var userLogin = _context.UserLogins.Include(x=>x.User).Where(U => U.RoleId == 2).ToList();
            ViewBag.contact = _context.Contacts.ToList();

            var homeModels = Tuple.Create<IEnumerable<Category>, IEnumerable< Tetstimonial >, IEnumerable<Recipe>, IEnumerable<UserLogin>> (Categories, tetstimonials, receipes, userLogin);
            return View(homeModels);
        }
        [HttpPost]
        public IActionResult Contact(string text , string email)
        {
            Contact contact = new Contact();
            contact.Email = email;
            contact.Text = text;
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult chefByRecipe(int Id)//not working
        {
            var chefRecipes = _context.Recipes.Include(x=>x.User).Where(x=>x.UserId ==Id).ToList();
            return View(chefRecipes);
        }
        public IActionResult GetProductsByCategoryId(int Id)
        {
            var Recipes = _context.Recipes.Include(x=>x.Category).Where(x => x.CategoryId == Id).ToList();

            return View(Recipes);
        }
        public IActionResult Payment()
        {
            //var soled = _context.UserRecipes.ToList();
            //var user = _context.Users.ToList();
            //var recipe = _context.Recipes.ToList();
            
            //var payment = _context.Payments.ToList();
            //var result = from p in payment
            //             join r in recipe on p.Id equals r.Id
                     
            //             select new JoinTables { Payment = p, Recipe = r };
            return View();
        }
        [HttpPost]
        public async Task Payment(Payment payment , int Id , int Price)
        {
            var CustomerId = HttpContext.Session.GetInt32("CustomerId");
            
            var  users = await _context.Users.Where(x => x.Id == CustomerId).SingleOrDefaultAsync();

            var pay =await _context.Payments.Where(x => x.CardNumber == payment.CardNumber && x.ExDate >= payment.ExDate && x.Ccv == payment.Ccv).SingleOrDefaultAsync();
            if (pay != null)
            {
                //adding to UserRecipe Table 
                UserRecipe userRecipe = new UserRecipe();
                userRecipe.RecipeId = Id;
                userRecipe.UserId = CustomerId;
                userRecipe.ParchaseDate = DateTime.Now;
                _context.UserRecipes.Add(userRecipe);
                _context.SaveChanges();

                //create the pdf file 
                
                var recipe = await _context.Recipes.Include(x=>x.Category).Where(x=>x.Id == Id).SingleOrDefaultAsync();
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string fileName = recipe.ImagePath;
                string path = Path.Combine(wwwrootPath + "/Images/", fileName);
                string PdfFieleName = recipe.RecipeName + ".PDF";
                string PdfPath=Path.Combine(wwwrootPath +"/PDF/", PdfFieleName);

                 Document doc = new Document();
                if (!System.IO.File.Exists(PdfPath))
                {
                    FileStream fs = new FileStream(PdfPath, FileMode.Create);
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();
                    doc.Add(new Paragraph());
                    Paragraph paragraph = new Paragraph(recipe.Category.CategoryName );
                    doc.Add(paragraph);
                    doc.Add(new Paragraph());
                    Paragraph paragraph2 = new Paragraph((recipe.RecipeName + "_" + recipe.Ingrediients + "_" + recipe.Preparation+ "_" + recipe.Price));
                    doc.Add(paragraph2);
                    Image image = Image.GetInstance(path);
                    doc.Add(image);
                    doc.Close();
                    fs.Close(); 
                }
                var userEmail = users.Email;
                SendingEmailAsync(userEmail, PdfPath);

            }

           
        }
        [HttpPost]
       public void SendingEmailAsync(string userEmail, string PdfPath)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.ethereal.email");
            mail.From = new MailAddress("hassan58@ethereal.email");
            mail.To.Add(userEmail);
            mail.Subject = "Recipe";
            mail.Body = "Recipe Details : ";

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(PdfPath);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("hassan58@ethereal.email", "U8aXUbpmNCas1Egkb8");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
        public IActionResult ChefRecipe() //not working with to list but working with firstOrDefault
        {
            var ChefId = HttpContext.Session.GetInt32("ChefId");
            var ChefRecipes = _context.Recipes.Where(c=>c.UserId == ChefId).ToList();
            return View(ChefRecipes);
        }
        public IActionResult CreateRecipe()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe([Bind("Id,RecipeName,Ingrediients,Preparation,Descreption,Price,ImageFile,Status,CreatedDate,CategoryId")] Recipe recipe)
        {

            var ChefId = HttpContext.Session.GetInt32("ChefId");
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + recipe.ImageFile.FileName;
                string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {

                    recipe.ImageFile.CopyToAsync(fileStream);
                }
                recipe.UserId = ChefId;
                recipe.ImagePath = imageName;
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", recipe.UserId);
            return View(recipe);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
