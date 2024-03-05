namespace GDP_API.Models.DTOs
{
    public class DashboardStatisticsDTO
    {
        public int ActiveProjectsCount { get; set; }
        public int NormalUsersCount { get; set; }
        public int ExpertUsersCount { get; set; }
        public int ActivitiesCount { get; set; }
        public int OverdueActivitiesCount { get; set; }
        public IEnumerable<StatusCountDTO> ActivitiesCountByStatus { get; set; } = [];
    }
    public class StatusCountDTO
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}