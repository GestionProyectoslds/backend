using GDP_API.Data;
using GDP_API.Models;

public class ExpertUserRepository : IExpertUserRepository
{
    private readonly DataContext _context;

    public ExpertUserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ExpertUser> Add(ExpertUser expertUser)
    {
        await _context.ExpertUsers.AddAsync(expertUser);
        await _context.SaveChangesAsync();
        return expertUser;
    }

    public async Task<ExpertUser> GetByUserId(int userId)
    {
        return await _context.ExpertUsers.FindAsync(userId);
    }
}