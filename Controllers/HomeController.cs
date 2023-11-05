using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;

namespace ClinicalPharmaSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserRepository userRepository;

        public HomeController(ILogger<HomeController> logger, UserRepository userRepository)
        {
            _logger = logger;
            this.userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate the user's login credentials
                var user = userRepository.GetUserByUsername(model.UserName);

                if (user != null && user.Password == model.Password)
                {
                    // Successful login
                    return RedirectToAction("Clinical", "Dashboard");
                }
                else
                {
                    // Invalid login
                    TempData["failedlogin"] = "Invalid login attempt.";
                }
            }
            return View(model);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                userRepository.InsertUser(model);

                // Set a TempData message for successful registration
                TempData["RegistrationSuccessMessage"] = "Registration successful! Redirecting to the Login...";
            }
            return View(model);
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