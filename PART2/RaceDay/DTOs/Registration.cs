using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class Registration
    {
        [Required] 
        public string Username { get; set; }
        [Required]
        [EmailAddress] 
        public string UserEmail { get; set; }
        [Required] 
        public string UserPassword { get; set; }
        [Required] 
        public string UserRole { get; set; } // "Organiser" or "Participant"
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Phone { get; set; }
    }
}