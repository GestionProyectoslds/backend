    using Microsoft.EntityFrameworkCore;
using GDP_API.Models;
using GDP_API.Data;
public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    // Add the missing using directive

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task<User> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}