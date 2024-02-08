    using Microsoft.EntityFrameworkCore;
using GDP_API.Models;
using GDP_API.Data;
public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(DataContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
   

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task<User> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
     public async Task ConfirmEmail(User user)
    {

            user.Confirmed = true;
            user.Token = "";
            await _context.SaveChangesAsync();         
    }
    }
    
