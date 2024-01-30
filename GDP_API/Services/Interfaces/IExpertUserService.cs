using GDP_API.Models;

public interface IExpertUserService
{
    Task<ExpertUser> RegisterExpertUser(ExpertUserRegistrationDto expertUser);
    
}