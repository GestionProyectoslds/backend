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
    Task<int> ExpertGetActiveProjectsCount(int userId);
    Task<int> ExpertGetNormalUsersCount(int userId);
    Task<int> ExpertGetExpertUsersCount(int userId);
    Task<int> ExpertGetActivitiesCount(int userId);
    Task<int> ExpertGetOverdueActivitiesCount(int userId);
    Task<int> ExpertGetCompleteProjectsCount(int userId);
    Task<int> ExpertGetInProgressProjectsCount(int userId);
    Task<IEnumerable<StatusCountDTO>> ExpertGetActivitiesCountByStatus(int userId);
}