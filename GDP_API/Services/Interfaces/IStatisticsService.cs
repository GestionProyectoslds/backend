using GDP_API.Models.DTOs;

public interface IStatisticsService
{
    Task<DashboardStatisticsDTO> GetStatistics(string jwt);
}