using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class Results
    {
        [Key]   
        public int resultID { get; set; }
        [Required]
        public TimeOnly participantTime { get; set; }
        [Required]
        public string participantPosition { get; set; }

        // get the foreign key from events
        public int eventID { get; set; }
        [ForeignKey("eventID")]
        public EventGig EventGig { get; set; }

        // get the foreign key from participants
        public int participantID { get; set; }
        [ForeignKey("participantID")]
        public Participant Participant { get; set; }
    }
}