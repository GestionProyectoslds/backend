namespace GDP_API.Models.DTOs
{
    public class ProjectCreationDTO
    {
        public required string Name { get; set; } = string.Empty;
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required decimal Budget { get; set; }
        public required decimal Cost { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Cost { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
    public class ProjectFilterDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MaxBudget { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxCost { get; set; }
        public decimal? MinCost { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string? UserEmail { get; set; } = string.Empty;
        //TODO: Implement CATEGORY @IsaacCruz
        public string? CategoryName { get; set; } = string.Empty;
    }
    public class UserProjectLinkDto
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
    public class LinkProjectCategoryDTO
    {
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
    }
    public class ProjectCategoryCreationDTO
    {
        public string Name { get; set; } = string.Empty;
    }
}
