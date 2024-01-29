namespace GDP_API.Models.DTOs
{
    public class RegisterDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
    public class EmailLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
