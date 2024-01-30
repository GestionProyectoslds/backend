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
[Route("api/[controller]")]
[ApiController]
public class ExpertUserController : ControllerBase
{
    private readonly IExpertUserService _service;

    public ExpertUserController(IExpertUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public IActionResult Register(ExpertUserRegistrationDto expertUser)
    {
        _service.RegisterExpertUser(expertUser);
        return Ok(expertUser);
    }

   
}