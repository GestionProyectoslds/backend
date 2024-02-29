using GDP_API;

namespace GDP_API.Models.DTOs
{
    public class ActivityDto
    {
        public string Description { get; set; } = string.Empty;
        public string AcceptanceCriteria { get; set; } = string.Empty;
        public string RequestedChanges { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Blockers { get; set; } = string.Empty;
        public ActivityPriority Priority { get; set; }
        public int ProjectId { get; set; }
    }
    public class ActivityFilterDto
    {
        public int? Id { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? AcceptanceCriteria { get; set; } = string.Empty;
        public string? RequestedChanges { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Blockers { get; set; } = string.Empty;
        public ActivityPriority? Priority { get; set; }
        public int? ProjectId { get; set; }
    }

    public class LinkActivityDto
    {
        public int UserId { get; set; }
        public int ActivityId { get; set; }
    }
    public class PostAOverdue()
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Overdue { get; set; }

    }
    public class PostAStatus()
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int Quantity { get; set; }

    }
}
