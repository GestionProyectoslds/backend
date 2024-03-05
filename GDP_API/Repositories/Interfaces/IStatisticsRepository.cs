using GDP_API.Models.DTOs;

public interface IStatisticsRepository
{
    Task<int> GetActiveProjectsCount();
    Task<int> GetNormalUsersCount();
    Task<int> GetExpertUsersCount();
    Task<int> GetActivitiesCount();
    Task<int> GetOverdueActivitiesCount();
    Task<IEnumerable<StatusCountDTO>> GetActivitiesCountByStatus();
}