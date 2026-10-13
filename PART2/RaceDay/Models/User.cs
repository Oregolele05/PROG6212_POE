using System.ComponentModel.DataAnnotations;

namespace RaceDay.Models
{
    public class User
    {
        [Key]
        public int userID { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string userEmail { get; set; }
        [Required]
        public string userRole { get; set; }
    }
}