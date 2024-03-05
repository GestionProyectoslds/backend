using GDP_API.Models.DTOs;

public interface IStatisticsService
{
    DashboardStatisticsDTO? GetStatistics();
}