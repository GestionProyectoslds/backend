using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieves all users from the repository.
    /// </summary>
    /// <returns>A list of User objects.</returns>
    public async Task<List<User>> GetAllUsers()
    {
        return await _repository.GetAllUsers();
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    public async Task<User> GetUser(int id)
    {
        return await _repository.GetUser(id);
    }


    public async Task<User> Register(RegisterDTO request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        User user = new User { 
            Email = request.Email, 
            Password = passwordHash,
            Name = request.Name,
            Surname = request.Surname,
        };
        return await _repository.AddUser(user);
       
        
    }
    public async Task<string> Login(string email, string password)
    {
        var user = await _repository.GetUserByEmail(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _repository.GetUserByEmail(email);
    }
}