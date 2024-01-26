using GDP_API.Data;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GDP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public UserController(IConfiguration configuration, DataContext context) {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers() { 
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }


        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.email = request.Username;
            user.Password = passwordHash;
            /* write user to database*/
            return Ok(user);
        }
        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO request)
        {
            
            // Revisar en bd si el usuario existe
            if(user.email != request.Username)
            {
                return BadRequest("Ayy lmao");
            }
            // Revisar si el hash de la contraseña de la petición corresponde al usuario
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
            // Incorrect password
                return Unauthorized("XD");
            }
            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> { 
            new Claim(ClaimTypes.Name, user.email),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "User")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration
                .GetSection("AppSettings:Token")
                .Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                ) ;
            var jwt = new JwtSecurityTokenHandler().WriteToken(token) ;
            return jwt;
        }
    }
}
