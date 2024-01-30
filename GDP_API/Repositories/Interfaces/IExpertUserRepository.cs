using GDP_API.Models;
public interface IExpertUserRepository
{
    Task<ExpertUser> Add(ExpertUser expertUser);
    
}