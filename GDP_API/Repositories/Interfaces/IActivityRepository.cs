
using GDP_API.Models;

public interface IActivityRepository
{
    Task<Activity> CreateActivity(Activity activity);
    Task<List<Activity>> GetActivities();
    Task<Activity?> GetActivity(int id);
    Task<List<Activity>> GetActivitiesByUser(int id);
    Task<Activity> UpdateActivity(Activity activity);
    Task DeleteActivity(int id);
    Task LinkUserToActivity(int userId, int activityId);
    Task UnlinkUserFromActivity(int userId, int activityId);
}