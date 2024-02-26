namespace GDP_API.Models
{
    public class ProjectHasCategory
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int CategoryId { get; set; }
        public ProjectCategory Category { get; set; }
    }
}