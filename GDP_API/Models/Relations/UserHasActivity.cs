namespace GDP_API.Models
{
    public class UserHasActivity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        
    }
}