using BulkyBookApp.Data;
using BulkyBookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            //bool isAdmin = User.IsInRole("Admin"); // Check if the current user is in the "Admin" role
            //ViewBag.IsAdmin = isAdmin; // Pass the value to the view
            return View();
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

        //GET
        public IActionResult login()
        {
            Userclass _loginModel = new Userclass();
            return View(_loginModel);
        }

        //POST
        [HttpPost]
        public IActionResult login(Userclass loginModel)
        {
            byte[] key = Common.Encryption.GenerateKey();
            byte[] iv = Common.Encryption.GenerateIV();
            loginModel.Password = Common.Encryption.Encrypt(loginModel.Password, key, iv);

            // Retrieve the user based on the encrypted password and email
            var status = _context.User
                         .Include(u => u.Roles)  // Ensure roles are eagerly loaded
                         .FirstOrDefault(x => x.Email == loginModel.Email && x.Password == loginModel.Password);

            if (status != null)
            {
                // Retrieve roles
                var userRoles = status.Roles.Select(r => r.RoleName).ToList();
                HttpContext.Session.SetString("UserRole", string.Join(",", userRoles));

                // Check and handle the roles
                if (userRoles.Contains("Admin"))
                {
                    TempData["Success"] = "Admin Login successful!";
                    return RedirectToAction("ProductIndex", "Product");
                }
                else
                {
                    TempData["Success"] = "User Login successful!";
                    return RedirectToAction("ProductIndex", "Product");
                }
            }

            // If the user is not found or login fails
            TempData["Error"] = "Invalid login credentials!";
            return RedirectToAction("Index", "Home");
        }



    }
}
