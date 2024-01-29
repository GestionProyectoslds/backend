

using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(int id);
    Task<User> Register(UserDTO request);
}