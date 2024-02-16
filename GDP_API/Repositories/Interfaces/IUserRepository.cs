using GDP_API.Models;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> AddUser(User user);
    Task ConfirmEmail(User user);
    Task SetOtp(User user, string otp);
    Task ResetOtp(User user);
    Task ResetPassword(User user, string password);
}