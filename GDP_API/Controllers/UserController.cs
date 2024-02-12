using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet(Name="All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Normal, Expert")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetUser(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        [HttpGet("email/{email}")] 
        [Authorize]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _service.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpGet("email/confirm")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            try
            {
                await _service.ConfirmEmail(email, token);
                return Ok("Email confirmed");
            }
            catch (Exception ex)
            {
               
                return BadRequest(new { message = "Email confirmation failed", error = ex.Message });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO request)
        {
            try
            {
                var registrationResult = await _service.RegisterExpert(request);
                var userType = request.UserTypeId == UserType.Expert ? "Expert " : "";
                return Ok($"{registrationResult.User} {userType}User Registered");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(EmailLoginDTO request, bool otp= false)
        {
            try{
                var token = await _service.Login(request.Email, request.Password, otp);
                if (token == null)
                {
                    return BadRequest(new { message = "Invalid credentials" });
                }
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Login failed", error = ex.Message });
            
        }
        }
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp(string email)
    {
        try
        {
            await _service.RequestOtp(email);
            return Ok(new { message = "OTP sent." });
        }
        catch (Exception ex)
        {
            // Log the exception
            return BadRequest(new { message = ex.Message });
        }
    }
            
    }
}
