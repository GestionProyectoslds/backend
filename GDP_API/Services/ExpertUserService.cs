using GDP_API.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class ExpertUserService : IExpertUserService
{
    private readonly IExpertUserRepository _repository;
    private readonly IUserRepository _userRepo;
    
    private readonly ILogger<ExpertUserService> _logger;

    public ExpertUserService(IExpertUserRepository repository, IUserRepository userRepo, ILogger<ExpertUserService> logger)
    {
        _repository = repository;
        _userRepo = userRepo;
        _logger = logger;
    }

    public async Task<ExpertUser> RegisterExpertUser(ExpertUserRegistrationDto registrationDTO)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDTO.UserDTO.Password);

        User registeredUser = new User 
        { 
            Name = registrationDTO.UserDTO.Name,
            Surname = registrationDTO.UserDTO.Surname,
            Email = registrationDTO.UserDTO.Email, 
            PhoneNumber = registrationDTO.UserDTO.PhoneNumber,
            Password = passwordHash,
            TermsAccepted = registrationDTO.UserDTO.TermsAccepted,
            CreatedDate = DateTime.UtcNow,
            UserTypeId = GDP_API.UserType.Normal
        };

        var user = await _userRepo.AddUser(registeredUser);

        ExpertUser expertUser = new ExpertUser
        {
            UserId = user.Id,
            User = user,
            Address = registrationDTO.Address,
            City = registrationDTO.City,
            State = registrationDTO.State,
            Country = registrationDTO.Country,
            EducationLevel = registrationDTO.EducationLevel
        };

        return await _repository.Add(expertUser);
    }
}