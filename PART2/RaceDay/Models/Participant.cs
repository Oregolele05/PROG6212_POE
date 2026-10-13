using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class Participant
    {
        [Key]
        public int participantID { get; set; }
        [Required]
        public string participantName { get; set; }
        [Required]
        public string participantEmail { get; set; }
        [Required]
        public string participantPhone { get; set; }

        //get the foreign key from Users

        public int userID { get; set; }
        [ForeignKey("userID")]
        public User User { get; set; }
    }
}