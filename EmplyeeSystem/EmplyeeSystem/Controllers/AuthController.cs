using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EmplyeeSystem.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace EmplyeeSystem.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly string? jwtKey;
        private readonly string? _issuer;
        private readonly string? _audience;
        private readonly int _expiry;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            jwtKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expiry = int.Parse(configuration["Jwt:ExpiryMinutes"]);
        }

        // GET: /Auth/Signup
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // POST: /Auth/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(Registration model)
        {
            if (ModelState.IsValid)
            {
                var isExistUser = await _userManager.FindByEmailAsync(model.Email);
                if (isExistUser != null)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                };

                var res = await _userManager.CreateAsync(user, model.Password);
                if (!res.Succeeded)
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Auth/Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or email.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid credentials.");
                }
            }

            return View(model);
        }

        // GET: /Auth/Logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
