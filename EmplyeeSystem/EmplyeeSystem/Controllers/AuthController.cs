using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmplyeeSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        public AuthController(UserManager<User> user,SignInManager<User> signInManager,IConfiguration configuration)
        {
            
           _userManager = user;
            _signInManager=signInManager;
            jwtKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expiry = int.Parse(configuration["Jwt:ExpiryMinutes"]);
            
        }
        [HttpPost("signup")]
        public async Task<IActionResult> Registration([FromBody] Registration model)
        {
            try
            {
                if (model == null || model.FirstName == null || model.LastName == null || model.Email == null || model.Password == null)
                {
                    return BadRequest("All fields are required!");
                }
                var isExistUser = await _userManager.FindByEmailAsync(model.Email);
                if (isExistUser != null)
                {
                    return Conflict("An account with this email already exists.");
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
                    return BadRequest(errors);
                }
                return Ok("Registration completed successfully!");
            }
            catch (Exception ex) { 
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginModel model)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync(model.email);
                if (user == null) return Unauthorized(new { success = false, message = "Ivalid Username or email" });
                var result = _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.IsCanceled) return Unauthorized(new { success = false, message = "invalid credentials...." });

                var token = GenerateToke(user);
                return Ok(new { success = true, token });

            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        private String GenerateToke(User user)
        {
            var claims = new[] { 
                new Claim(JwtRegisteredClaimNames.Sub, user.Id) ,
                new Claim(JwtRegisteredClaimNames.Email, user.Email) ,
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) ,
                new Claim("Name",   user.Email) ,
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(_expiry), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("logout successfully");
        }
    }

    }
