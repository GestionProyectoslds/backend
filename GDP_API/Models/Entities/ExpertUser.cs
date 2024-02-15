namespace GDP_API.Models
{
    public class ExpertUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        /// <summary>
        /// CV File URL Location
        /// </summary>
        public string CVPath { get; set; } = string.Empty;
        public string LinkedInURI { get; set; } = string.Empty;
    }
}

