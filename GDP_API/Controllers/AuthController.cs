using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GDP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration) {
            _configuration = configuration;
        }
        
        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.Password = passwordHash;
            /* write user to database*/
            return Ok(user);
        }
        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO request)
        {
            
            // Revisar en bd si el usuario existe
            if(user.Username != request.Username)
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
            new Claim(ClaimTypes.Name, user.Username)
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
