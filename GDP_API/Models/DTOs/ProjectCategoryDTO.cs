namespace GDP_API.Models.DTOs
{
    public class ProjectCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class ProjectCategoryFilterDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = string.Empty;
    }
}
