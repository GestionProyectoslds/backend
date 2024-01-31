using GDP_API;
using GDP_API.Models;

public class ExpertUserService: IExpertUserService
{
    private readonly IExpertUserRepository _repository;
    private readonly IUserService _userService;


    public ExpertUserService(IExpertUserRepository userRepository, IUserService userService)
    {
        _repository = userRepository;
        _userService = userService;
    }

    public async Task RegisterExpertUser(ExpertUserRegistrationDto expertUserDto)
    {
        var registeredUser= await _userService.Register(expertUserDto.UserDTO, UserType.Expert);

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
        await _repository.Add(expertUser);
    }
}