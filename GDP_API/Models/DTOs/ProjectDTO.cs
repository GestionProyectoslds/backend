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

    public class UserProjectLinkDto
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
