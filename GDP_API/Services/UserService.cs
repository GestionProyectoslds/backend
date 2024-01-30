using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.IdentityModel.Tokens;


public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    public UserService(IUserRepository repository, IConfiguration configuration, ILogger<UserService> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
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


    public async Task<User> Register(UserRegistrationDTO registrationDTO)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password);
       
        User user = new User 
    { 
        Name = registrationDTO.Name,
        Surname = registrationDTO.Surname,
        Email = registrationDTO.Email, 
        PhoneNumber = registrationDTO.PhoneNumber,
        Password = passwordHash,
        // Restore = request.Restore,
        // Confirmed = request.Confirmed,
        // Token = request.Token,
        TermsAccepted = registrationDTO.TermsAccepted,
        CreatedDate = DateTime.UtcNow,
        UserTypeId = GDP_API.UserType.Normal
    };
//     string userProperties = $"Name: {user.Name}, Surname: {user.Surname}, Email: {user.Email}, PhoneNumber: {user.PhoneNumber}, Password: {user.Password}, TermsAccepted: {user.TermsAccepted}, CreatedDate: {user.CreatedDate}, UserTypeId: {user.UserTypeId}";
// _logger.LogInformation("User properties: {UserProperties}", userProperties);

    var res = await _repository.AddUser(user);
   
        return res;
       
        
    }
    public async Task<string> Login(string email, string password)
    {
        var user = await _repository.GetUserByEmail(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        //TODO - Use Azure keystore for production
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                 new Claim(ClaimTypes.Name, user.Id.ToString()),
                 new Claim(ClaimTypes.Role, user.UserTypeId.ToString()) }),
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