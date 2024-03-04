using GDP_API.Data;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;


public class ActivityRepository : IActivityRepository
{
    private readonly DataContext _context;
    private readonly ILogger<ActivityRepository> _logger;
    const string NF = "Activity not found";
    const string NotLinked = "User is not linked to activity";
    const string Linked = "User is already linked to activity";
    const string NoFilter = "At least one filter property must be set";
    public ActivityRepository(DataContext context, ILogger<ActivityRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Activity> CreateActivity(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<List<Activity>> GetActivities() => await _context.Activities.ToListAsync();
    public async Task<Activity?> GetActivity(int id) => await _context.Activities.FindAsync(id);

    public async Task<Activity> UpdateActivity(Activity activity)
    {
        _context.Entry(activity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task DeleteActivity(int id)
    {
        Activity? activity = await _context.Activities.FindAsync(id);
        bool activityIsNull = activity is null;
        if (activityIsNull)
        {
            throw new KeyNotFoundException(NF);
        }
        _context.Activities.Remove(activity!);
        await _context.SaveChangesAsync();
    }

    public async Task LinkUserToActivity(int userId, int activityId)
    {
        var userHasActivity = new UserHasActivity
        {
            UserId = userId,
            ActivityId = activityId
        };
        if (await _context.UserHasActivities.ContainsAsync(userHasActivity))
        {
            throw new DbUpdateException(Linked);
        }
        await _context.UserHasActivities.AddAsync(userHasActivity);
        await _context.SaveChangesAsync();
    }

    public async Task UnlinkUserFromActivity(int userId, int activityId)
    {
        _logger.LogInformation($"Unlinking user {userId} from activity {activityId}");
        var userHasActivity = new UserHasActivity
        {
            UserId = userId,
            ActivityId = activityId
        };
        if (!await _context.UserHasActivities.ContainsAsync(userHasActivity))
        {
            throw new KeyNotFoundException(NotLinked);
        }
        _context.UserHasActivities.Remove(userHasActivity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Activity>> GetActivitiesByUser(int userId)
    {
        return await _context.UserHasActivities
       .Where(ua => ua.UserId == userId)
       .Select(ua => ua.Activity)
       .ToListAsync();
    }

    public async Task<List<Activity>> FilterActivities(ActivityFilterDto filter)
    {
        // Check if all properties of the filter are null
        if (filter.GetType().GetProperties().All(prop => prop.GetValue(filter) == null))
        {
            // If all properties are null, return an empty list
            throw new ArgumentException(NoFilter);
        }
        var query = _context.Activities.AsQueryable();

        if (filter.Id.HasValue)
        {
            query = query.Where(a => a.Id == filter.Id.Value);
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query = query.Where(a => a.Description.Contains(filter.Description));
        }

        if (!string.IsNullOrEmpty(filter.AcceptanceCriteria))
        {
            query = query.Where(a => a.AcceptanceCriteria.Contains(filter.AcceptanceCriteria));
        }

        if (!string.IsNullOrEmpty(filter.RequestedChanges))
        {
            query = query.Where(a => a.RequestedChanges.Contains(filter.RequestedChanges));
        }

        if (!string.IsNullOrEmpty(filter.Status))
        {
            query = query.Where(a => a.Status.Contains(filter.Status));
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(a => a.StartDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(a => a.EndDate <= filter.EndDate.Value);
        }

        if (!string.IsNullOrEmpty(filter.Blockers))
        {
            query = query.Where(a => a.Blockers.Contains(filter.Blockers));
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(a => a.Priority == filter.Priority.Value);
        }

        if (filter.ProjectId.HasValue)
        {
            query = query.Where(a => a.ProjectId == filter.ProjectId.Value);
        }

        return await query.ToListAsync();
    }
}