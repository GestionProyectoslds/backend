namespace GDP_API.Models.DTOs
{
    public class RegisterDTO
    {
       public required string Name { get; set; } = string.Empty;
        public required string Surname { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string ConfirmPassword { get; set; } = string.Empty;
        // public required bool Restore { get; set; }
        // public required bool Confirmed { get; set; } 
        // public required string Token { get; set; } = string.Empty;
        public bool TermsAccepted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserTypeId { get; set; }
    }
    public class EmailLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
