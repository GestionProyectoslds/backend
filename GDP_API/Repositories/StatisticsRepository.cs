using GDP_API;
using GDP_API.Data;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly DataContext _context;
    private readonly ILogger<StatisticsRepository> _logger;
    private readonly IUserService _userService;

    public StatisticsRepository(DataContext context, ILogger<StatisticsRepository> logger, IUserService userService)
    {
        _context = context;
        _logger = logger;
        _userService = userService;
    }
    public async Task<int> GetActiveProjectsCount()
    {
        var count = await _context.Projects.CountAsync(p => p.IsActive);
        _logger.LogInformation($"GetActiveProjectsCount returned {count}");
        return count;
    }
    public async Task<int> GetCompleteProjectsCount()
    {
        var count = await _context.Projects.CountAsync(p => p.IsComplete);
        _logger.LogInformation($"GetCompleteProjectsCount returned {count}");
        return count;
    }
    public async Task<int> GetInProgressProjectsCount()
    {
        var count = await _context.Projects.CountAsync(p => p.IsActive && !p.IsComplete);
        _logger.LogInformation($"GetActiveNotCompletedProjectsCount returned {count}");
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
    //consultas expertos
    public async Task<int> GetActiveProjectsCount(int userId)
    {
        var query = from user in _context.Users
                    join uhp in _context.UserHasProjects on user.Id equals uhp.UserId
                    join project in _context.Projects on uhp.ProjectId equals project.Id
                    where user.Id == userId
                    select new
                    {
                        projectID = project.Id,
                        project.IsActive
                    };

        var count = await query.CountAsync(p => p.IsActive);
        _logger.LogInformation($"ExpertGetActiveProjectsCount returned {count}");
        return count;
    }
    public async Task<int> GetCompleteProjectsCount(int userId)
    {
        var query = from user in _context.Users
                    join uhp in _context.UserHasProjects on user.Id equals uhp.UserId
                    join project in _context.Projects on uhp.ProjectId equals project.Id
                    where user.Id == userId
                    select new
                    {
                        projectID = project.Id,
                        project.IsComplete
                    };
        var count = await query.CountAsync(p => p.IsComplete);
        _logger.LogInformation($"GetCompleteProjectsCount returned {count}");
        return count;
    }
    public async Task<int> GetInProgressProjectsCount(int userId)
    {
        var query = from user in _context.Users
                    join uhp in _context.UserHasProjects on user.Id equals uhp.UserId
                    join project in _context.Projects on uhp.ProjectId equals project.Id
                    where user.Id == userId
                    select new
                    {
                        projectID = project.Id,
                        project.IsActive,
                        project.IsComplete
                    };
        var count = await query.CountAsync(p => p.IsActive && !p.IsComplete);
        _logger.LogInformation($"ExpertGetInProgressProjectsCount returned {count}");
        return count;
    }

    public async Task<int> GetNormalUsersCount(int userId)
    {
        var userProjects = _context.UserHasProjects
            .Where(uhp => uhp.UserId == userId)
            .Select(uhp => uhp.ProjectId);

        var count = await _context.Users
            .Where(u => u.UserTypeId == UserType.Normal)
            .Join(_context.UserHasProjects,
                  user => user.Id,
                  uhp => uhp.UserId,
                  (user, uhp) => new { User = user, UserHasProjects = uhp })
            .Where(joined => userProjects.Contains(joined.UserHasProjects.ProjectId))
            .CountAsync();

        _logger.LogInformation($"ExpertGetNormalUsersCount returned {count}");
        return count;
    }

    public async Task<int> GetExpertUsersCount(int userId)
    {
        var userProjects = _context.UserHasProjects
            .Where(uhp => uhp.UserId == userId)
            .Select(uhp => uhp.ProjectId);

        var count = await _context.Users
            .Where(u => u.UserTypeId == UserType.Expert)
            .Join(_context.UserHasProjects,
                  user => user.Id,
                  uhp => uhp.UserId,
                  (user, uhp) => new { User = user, UserHasProjects = uhp })
            .Where(joined => userProjects.Contains(joined.UserHasProjects.ProjectId))
            .CountAsync();

        _logger.LogInformation($"ExpertGetExpertUsersCount returned {count}");
        return count;
    }

    public async Task<int> GetActivitiesCount(int userId)
    {
        var query = from user in _context.Users
                    join uha in _context.UserHasActivities on user.Id equals uha.UserId
                    join activities in _context.Activities on uha.ActivityId equals activities.Id
                    where user.Id == userId
                    select new
                    {
                        activitiesId = activities.Id
                    };
        return await query.CountAsync();
    }

    public async Task<int> GetOverdueActivitiesCount(int userId)
    {
        var query = from user in _context.Users
                    join uha in _context.UserHasActivities on user.Id equals uha.UserId
                    join activities in _context.Activities on uha.ActivityId equals activities.Id
                    where user.Id == userId
                    select new
                    {
                        activitiesId = activities.Id,
                        activities.EndDate
                    };
        return await query.CountAsync(t => t.EndDate < DateTime.Now);
    }
    public async Task<IEnumerable<StatusCountDTO>> GetActivitiesCountByStatus(int userId)
    {
        var query = from user in _context.Users
                    join uha in _context.UserHasActivities on user.Id equals uha.UserId
                    join activities in _context.Activities on uha.ActivityId equals activities.Id
                    where user.Id == userId
                    select new
                    {
                        activitiesId = activities.Id,
                        activities.Status
                    };
        var result = await query
            .GroupBy(t => t.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        return result.Select(r => new StatusCountDTO { Status = r.Status, Count = r.Count });
    }
}