using System.ComponentModel.DataAnnotations.Schema;

namespace GDP_API.Models
{
    
      public class ExpertUser
    {
        public int Id { get; set; }
       
        public int UserId { get; set; } 
        public User User { get; set; }

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string EducationLevel { get; set; } = string.Empty;

        public string LearningMethod { get; set; } = string.Empty;

        public string CVPath { get; set; } = string.Empty; // Para almacenar la ruta del archivo CV

        //public byte[] CVContent { get; set; } // Para almacenar el contenido del CV en formato binario

        //public string CVExtension { get; set; } // Para almacenar la extensión del archivo CV
        public string LinkedInURI { get; set; } = string.Empty;
}
    }
    
