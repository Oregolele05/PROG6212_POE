using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class Login
    {
        [Required]
        [EmailAddress] 
        public string UserEmail { get; set; }
        [Required] 
        public string UserPassword { get; set; }
    }
}