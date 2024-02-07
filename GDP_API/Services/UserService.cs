using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IExpertUserRepository _expertRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    private readonly IEmailService _emailService;
    public UserService(IUserRepository repository, 
    IConfiguration configuration, ILogger<UserService> logger,
     IExpertUserRepository expertRepository, IEmailService emailService)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
        _expertRepository = expertRepository;
        _emailService = emailService;
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
        try
    {
        // Check if a user with the same email already exists
        var existingUser = await _repository.GetUserByEmail(registrationDTO.Email);
        if (existingUser != null)
        {
            throw new Exception("An account with this email already exists.");
        }
         string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDTO.Password);
         string token = Guid.NewGuid().ToString();
         string tokenHash = BCrypt.Net.BCrypt.HashPassword(token);
        const string emailMessage = "You have successfully registered. Please confirm your email address to complete the registration process.";
        User user = new User 
        { 
        Name = registrationDTO.Name,
        Surname = registrationDTO.Surname,
        Email = registrationDTO.Email, 
        PhoneNumber = registrationDTO.PhoneNumber,
        Password = passwordHash,
        TermsAccepted = registrationDTO.TermsAccepted,
        Token = tokenHash,
        CreatedDate = DateTime.UtcNow,
        UserTypeId = registrationDTO.UserTypeId
        };
    
        //TODO (maybe) - Change token so it can be verified without storing it
        //TODO - Change the URL so it adapts to the environment
        
            await _emailService.SendEmailAsync("montoyasotodanielxd@gmail.com", "Welcome to GDP",
            $" <a href='https://localhost:5153/api/User/confirmemail?email={user.Email}&token={token}'>Confirm Email</a> a");
     return await _repository.AddUser(user);
    }
    catch (DbUpdateException ex)
    {
        if (ex.InnerException != null && ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new Exception("An account with this email already exists.");
        }
        throw;
    }
       
         
    }
    public async Task<ExpertUser> RegisterExpert(UserRegistrationDTO registrationDTO){
        try{
            
            var user = await Register(registrationDTO);
            ExpertUser expert = new ExpertUser
            {
                UserId = user.Id,
                User = user,
                Experience = registrationDTO.Experience ?? "",
                EducationLevel = registrationDTO.EducationLevel ?? "",
                CVPath= registrationDTO.CVPath ?? "",
                LinkedInURI = registrationDTO.LinkedInURI ?? ""
            };
            
        return await _expertRepository.Add(expert);
        }catch(Exception){
            throw ;
        }

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
    public async Task ConfirmEmail(string email, string token)
    {

        await _repository.ConfirmEmail(email, token);
    }
}