using GDP_API.Models.DTOs;

public interface IStatisticsRepository
{
    Task<int> GetActiveProjectsCount();
    Task<int> GetNormalUsersCount();
    Task<int> GetExpertUsersCount();
    Task<int> GetActivitiesCount();
    Task<int> GetOverdueActivitiesCount();
    Task<int> GetCompleteProjectsCount();
    Task<int> GetInProgressProjectsCount();
    Task<IEnumerable<StatusCountDTO>> GetActivitiesCountByStatus();
    Task<int> GetActiveProjectsCount(int userId);
    Task<int> GetNormalUsersCount(int userId);
    Task<int> GetExpertUsersCount(int userId);
    Task<int> GetActivitiesCount(int userId);
    Task<int> GetOverdueActivitiesCount(int userId);
    Task<int> GetCompleteProjectsCount(int userId);
    Task<int> GetInProgressProjectsCount(int userId);
    Task<IEnumerable<StatusCountDTO>> GetActivitiesCountByStatus(int userId);
}