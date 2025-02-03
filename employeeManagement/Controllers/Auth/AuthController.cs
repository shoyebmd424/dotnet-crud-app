using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using employeeManagement.Models;
using employeeManagement.Controllers;
using employeeManagement.Model;

namespace EmplyeeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: /Auth/Signup - Returns the registration page
        [HttpGet("signup")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromForm] Registration model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "All fields are required!");
                    return View(model);
                }

                var isExistUser = await _userManager.FindByEmailAsync(model.Email);
                if (isExistUser != null)
                {
                    ModelState.AddModelError("", "An account with this email already exists.");
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
                    var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                    ModelState.AddModelError("", errors);
                    return View(model); 
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

     
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "Email and Password are required.");
                    return View(model); 
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or email.");
                    return View(model); 
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid credentials.");
                    return View(model); 
                }

                var token = GenerateToken(user);
                return RedirectToAction("Index", "Home"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(model); 
            }
        }

        // Method to generate JWT token
        private string GenerateToken(User user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Name", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiry),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // POST: /Auth/Logout - Handles user logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
