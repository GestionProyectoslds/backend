using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;


public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retrieves all users from the repository.
    /// </summary>
    /// <returns>A list of User objects.</returns>
    public async Task<List<User>> GetAllUsers()
    {
        return await _repository.GetAllUsers();
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    public async Task<User> GetUser(int id)
    {
        return await _repository.GetUser(id);
    }


    public async Task<User> Register(UserDTO request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        User user = new User { 
            Email = request.Email, 
            Password = passwordHash,
            Name = request.Name,
            Surname = request.Surname,
        };
        return await _repository.AddUser(user);
       
        
    }
}