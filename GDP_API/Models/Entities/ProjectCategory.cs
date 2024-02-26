namespace GDP_API.Models
{
    public class ProjectCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProjectHasCategory> ProjectHasCategories { get; set; }
    }
}