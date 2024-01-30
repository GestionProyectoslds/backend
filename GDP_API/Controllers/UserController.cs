 // Add the missing using directive
using GDP_API.Data;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
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

    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("All"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers() 
    {
        var users = await _service.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}"),Authorize(Roles = "Normal, Expert")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _service.GetUser(id);
        if(user is null)
        {
            return NotFound("User not found");
        }
        return Ok(user);
    }
    [HttpGet("/email/{email}"), Authorize()]
    public async Task<IActionResult> GetUserByEmail(string email)
    {   
    var user = await _service.GetUserByEmail(email);

    if (user == null)
    {
        return NotFound();
    }

    return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationDTO request)
    {
        var user = await _service.Register(request);
        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(EmailLoginDTO request)
    {
        var token = await _service.Login(request.Email, request.Password);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
}
}