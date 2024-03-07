using GDP_API;
using GDP_API.Data;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly DataContext _context;
    private readonly ILogger<StatisticsRepository> _logger;

    public StatisticsRepository(DataContext context, ILogger<StatisticsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<int> GetActiveProjectsCount()
    {
        var count = await _context.Projects.CountAsync(p => p.IsActive);
        _logger.LogInformation($"GetActiveProjectsCount returned {count}");
        return count;
    }

    public async Task<int> GetNormalUsersCount()
    {
        var count = await _context.Users.CountAsync(u => u.UserTypeId == UserType.Normal);
        _logger.LogInformation($"GetNormalUsersCount returned {count}");
        return count;
    }

    public async Task<int> GetExpertUsersCount()
    {
        var count = await _context.Users.CountAsync(u => u.UserTypeId == UserType.Expert);
        _logger.LogInformation($"GetExpertUsersCount returned {count}");
        return count;
    }

    public async Task<int> GetActivitiesCount()
    {
        return await _context.Activities.CountAsync();
    }

    public async Task<int> GetOverdueActivitiesCount()
    {
        return await _context.Activities.CountAsync(t => t.EndDate < DateTime.Now);
    }

    public async Task<IEnumerable<StatusCountDTO>> GetActivitiesCountByStatus()
    {
        var result = await _context.Activities
            .GroupBy(t => t.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        return result.Select(r => new StatusCountDTO { Status = r.Status, Count = r.Count });
    }


}