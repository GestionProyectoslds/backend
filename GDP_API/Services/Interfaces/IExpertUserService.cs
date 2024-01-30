using GDP_API.Models;

public interface IExpertUserService
{
    Task RegisterExpertUser(ExpertUserRegistrationDto expertUser);
    
}