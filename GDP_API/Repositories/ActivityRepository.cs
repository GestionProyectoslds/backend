using System.Runtime.InteropServices;
using GDP_API.Data;
using GDP_API.Models;
using Microsoft.EntityFrameworkCore;


public class ActivityRepository : IActivityRepository
{
    private readonly DataContext _context;
    private readonly ILogger<ActivityRepository> _logger;
    const string NF = "Activity not found";
    const string NotLinked = "User is not linked to activity";
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
}