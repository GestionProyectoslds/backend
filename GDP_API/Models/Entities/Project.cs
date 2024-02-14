using System.ComponentModel.DataAnnotations;

namespace GDP_API.Models
{
    
      public class Project
    {
        public int Id { get; set; }       
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public decimal Cost { get; set; }
        public string Comments { get; set; } = string.Empty;
        public ICollection<UserHasProject> UserHasProjects { get; set; }     
    }
    
}
