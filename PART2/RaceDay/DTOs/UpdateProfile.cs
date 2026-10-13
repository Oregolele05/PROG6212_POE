using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class UpdateProfile
    {
        [Required] 
        public string Username { get; set; }
        [Required]
        [EmailAddress] 
        public string UserEmail { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Phone { get; set; }
    }
}
