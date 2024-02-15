namespace GDP_API.Models.DTOs
{
    public class UserRegistrationDTO
    {
        public required string Name { get; set; } = string.Empty;
        public required string Surname { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public bool TermsAccepted { get; set; }
        public UserType UserTypeId { get; set; } = UserType.Normal;
        public string? Experience { get; set; } = string.Empty;
        public string? EducationLevel { get; set; } = string.Empty;
        public string? CVPath { get; set; } = string.Empty; // Para almacenar la ruta del archivo CV
        public string? LinkedInURI { get; set; } = string.Empty;
    }
    public class EmailLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class RequestOtpDTO
    {
        public required string Email { get; set; }
        public bool isReset { get; set; } = false;
    }
    public class PasswordResetDTO
    {
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
