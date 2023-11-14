using ClinicalPharmaSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace ClinicalPharmaSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserRepository userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(ILogger<HomeController> logger, UserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            this.userRepository = userRepository;
            this._contextAccessor = contextAccessor;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetUserByUsername(model.UserName);

                if (user != null && user.Password == model.Password)
                {
                    // Retrieve user roles
                    List<object> roles = new List<object>();
                    roles.Add(user.RoleName);

                    // Create claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

                    // Add role claims
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role,role.ToString()));
                    }

                    // Create identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );
                    HttpContext.Session.SetString("LoginSuccessMessage", "Success");
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Initial", !string.IsNullOrEmpty(user.UserName) ? user.UserName[0].ToString() : "");
                    // Redirect to the desired page after successful login
                    return RedirectToAction("Clinical", "Dashboard");
                }

                TempData["failedlogin"] = "Invalid login attempt.";
            }

            // If we reach here, something went wrong, redisplay the form
            return View(model);
        }



        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Validate the user's login credentials
        //        var user = userRepository.GetUserByUsername(model.UserName);

        //        if (user != null && user.Password == model.Password)
        //        {
        //            _contextAccessor.HttpContext.Session.SetString("Role", "");
        //            // Successful login
        //            return RedirectToAction("Clinical", "Dashboard");
        //        }
        //        else
        //        {
        //            // Invalid login
        //            TempData["failedlogin"] = "Invalid login attempt.";
        //        }
        //    }
        //    return View(model);
        //}

        public IActionResult Registration()
        {
            //var roles = userRepository.GetRolesFromDatabase();

            //var registerViewModel = new RegisterViewModel
            //{
            //    Role = roles
            //};

            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                int Statusid = userRepository.InsertUser(model);

                if (Statusid == 0)
                {
                    TempData["RegistrationSuccessMessage"] = "You are not Authorized to access this page!!";
                }
                else if (Statusid == 1)
                {
                    TempData["RegistrationSuccessMessage"] = "Registration failed. Please contact administrator!!";
                }
                else
                {
                    // Set a TempData message for successful registration
                    TempData["RegistrationSuccessMessage"] = "Registration successful! Please contact administrator to activate your account. Redirecting to the Login...";
                }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or any other page
            return RedirectToAction("Login", "Home");
        }
    }
}