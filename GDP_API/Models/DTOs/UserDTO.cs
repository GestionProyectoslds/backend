namespace GDP_API.Models.DTOs
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}
