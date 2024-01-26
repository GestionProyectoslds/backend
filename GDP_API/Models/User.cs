namespace GDP_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
