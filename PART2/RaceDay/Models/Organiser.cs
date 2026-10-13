using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class Organiser
    {
        [Key]
        public int organiserID { get; set; }
        [Required]
        public string organiserName { get; set; }
        [Required]
        public string organiserEmail { get; set; }
        [Required]
        public string organiserPhone { get; set; }

        //get the foreign key from Users
        public int userID { get; set; }
        [ForeignKey("userID")]
        public User User { get; set; }
    }
}