

using GDP_API;
using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> Register(UserRegistrationDTO request, UserType userType = UserType.Normal);
    Task<string> Login(string email, string password);

}