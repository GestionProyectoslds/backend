

namespace GDP_API.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AcceptanceCriteria { get; set; } = string.Empty;
        public string RequestedChanges { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Blockers { get; set; } = string.Empty;
        public ActivityPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public ICollection<UserHasActivity>? UserHasActivities { get; set; }
    }
}
