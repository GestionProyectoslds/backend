using System.Runtime.InteropServices;
using GDP_API.Data;
using GDP_API.Models;
using Microsoft.EntityFrameworkCore;


public class ActivityRepository : IActivityRepository
{
    private readonly DataContext _context;
    const string NF = "Activity not found";
    public ActivityRepository(DataContext context)
    {
        _context = context;
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

    public Task LinkUserToActivity(int userId, int activityId)
    {
        throw new NotImplementedException();
    }

    public Task UnlinkUserFromActivity(int userId, int activityId)
    {
        throw new NotImplementedException();
    }
}