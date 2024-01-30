using GDP_API.Models;

public class ExpertUserService: IExpertUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IExpertUserRepository _expertUserRepository;

    public ExpertUserService(IUserRepository userRepository, IExpertUserRepository expertUserRepository)
    {
        _userRepository = userRepository;
        _expertUserRepository = expertUserRepository;
    }

    public async Task RegisterExpertUser(ExpertUserRegistrationDto expertUserDto)
    {
         string passwordHash = BCrypt.Net.BCrypt.HashPassword(expertUserDto.UserDTO.Password);
       
        // Build and register User
        var user = new User
        {
            // Assuming UserRegistrationDTO has properties that match User
             Name = expertUserDto.UserDTO.Name,
        Surname = expertUserDto.UserDTO.Surname,
        Email = expertUserDto.UserDTO.Email, 
        PhoneNumber = expertUserDto.UserDTO.PhoneNumber,
        Password = passwordHash,
        // Restore = request.Restore,
        // Confirmed = request.Confirmed,
        // Token = request.Token,
        TermsAccepted = expertUserDto.UserDTO.TermsAccepted,
        CreatedDate = DateTime.UtcNow,
        UserTypeId = GDP_API.UserType.Expert
            // Add other properties as needed
        };
        var registeredUser = await _userRepository.AddUser(user);

        // Build and register ExpertUser
        var expertUser = new ExpertUser
        {
            UserId = registeredUser.Id,
            Address = expertUserDto.Address,
            City = expertUserDto.City,
            State = expertUserDto.State,
            Country = expertUserDto.Country,
            EducationLevel = expertUserDto.EducationLevel,
            LearningMethod = expertUserDto.LearningMethod,
            CVPath = expertUserDto.CVPath,
            LinkedInURI = expertUserDto.LinkedInURI,
            // Add other properties as needed
        };
        await _expertUserRepository.Add(expertUser);
    }
}