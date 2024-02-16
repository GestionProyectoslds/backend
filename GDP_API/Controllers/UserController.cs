using System.Security.Claims;
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
        private readonly ILogger<UserController> _logger;
        const string NF = "User not found";

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(Name = "All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetUser(id);
            if (user == null)
            {
                return NotFound();
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
                switch (request.UserTypeId)
                {
                    case UserType.Normal:
                        var user = await _service.Register(request);
                        return Ok(new { message = "Registration successful ", user.Email });
                    case UserType.Expert:
                        var expert = await _service.RegisterExpert(request);
                        return Ok(new { message = "Registration successful ", expert.User.Email });
                    default:
                        return BadRequest(new { message = "Invalid user type" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed", error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(EmailLoginDTO request, bool otp = false)
        {
            try
            {
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
        [HttpPost("requestOtp")]
        public async Task<IActionResult> RequestOtp(RequestOtpDTO request)
        {
            try
            {
                var response = await _service.RequestOtp(request.Email, request.IsReset);
                return Ok(new { message = response });
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetDTO newPassword)
        {
            try
            {
                // Get the user's ID from the JWT
                var jwt = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
                var userId = User.FindFirstValue(ClaimTypes.Name);
                if (userId == null)
                {
                    return BadRequest(new { message = "Invalid user" });
                }
                // Reset the password
                await _service.ResetPassword(jwt, newPassword.NewPassword, newPassword.ConfirmPassword);
                return Ok(new { message = "Password reset successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"{ex.Message}" });
            }
        }

        #region private methods
            private IActionResult NotFound()
            {
                _logger.LogError(NF);
                return NotFound(NF);
            }
        #endregion
    }
}
