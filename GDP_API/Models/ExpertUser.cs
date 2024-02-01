using System.ComponentModel.DataAnnotations.Schema;

namespace GDP_API.Models
{
    
      public class ExpertUser
    {
        public int Id { get; set; }
       
        public int UserId { get; set; } 
        public User User { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;

        public string CVPath { get; set; } = string.Empty; // Para almacenar la ruta del archivo CV

        public string LinkedInURI { get; set; } = string.Empty;
}
    }
    
