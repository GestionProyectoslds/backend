using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IActivityService
{
    Task<Activity> CreateActivity(ActivityDto activity);
    Task<List<Activity>> GetActivities();
    Task<Activity?> GetActivity(int id);
    Task<List<Activity>> GetActivitiesByUser(int userId);
    Task<List<Activity>> FilterActivities(ActivityFilterDto filter);
    Task<Activity> UpdateActivity(int id, ActivityDto activity);
    Task DeleteActivity(int id);
    Task LinkUserToActivity(int userId, int activityId);
    Task UnlinkUserFromActivity(int userId, int activityId);
    Task CActivityStatus(int id, string status, int quantity);
    Task CActivityOverDue(int id, int total, int overdue);
}