using System.ComponentModel.DataAnnotations;

namespace GDP_API.Models
{
    
      public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
        // public bool Restore { get; set; }
        public bool Confirmed { get; set; } 
        public string Token { get; set; } = string.Empty;
        public bool TermsAccepted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public UserType UserTypeId { get; set; }
        //public UserType UserType { get; set; }

    }
    
}
