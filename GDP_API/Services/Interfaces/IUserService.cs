using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> Register(UserRegistrationDTO request);
    Task<ExpertUser> RegisterExpert(UserRegistrationDTO request);
    Task<string> Login(string email, string password, bool otp = false);
    Task<string> RequestOtp(string email);
    Task ConfirmEmail(string email, string token);
    Task ResetPassword(string jwt, string newPassword, string confirm);
}