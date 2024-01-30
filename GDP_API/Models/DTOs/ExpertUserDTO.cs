using GDP_API.Models.DTOs;

namespace GDP_API.Models
{
    public class ExpertUserRegistrationDto
    {
        public UserRegistrationDTO UserDTO { get; set; } 
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string EducationLevel { get; set; } = string.Empty;

        public string LearningMethod { get; set; } = string.Empty;

        public string CVPath { get; set; } = string.Empty; // Para almacenar la ruta del archivo CV
        public string LinkedInURI { get; set; } = string.Empty;
    }
}