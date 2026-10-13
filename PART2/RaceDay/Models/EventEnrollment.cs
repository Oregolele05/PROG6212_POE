using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class EventEnrollment
    {
        [Key]
        public int enrollmentID { get; set; }
        public DateOnly enrollmentDate { get; set; }

        //get the foreign key from EventGig
        public int eventID { get; set; }
        [ForeignKey("eventID")]
        public EventGig EventGig { get; set; }

        //get foreign key from Participant
        public int participantID { get; set; }
        [ForeignKey("participantID")]
        public Participant Participant { get; set; }

        //get foreign key from Categories
        public int categoryID { get; set; }
        [ForeignKey("categoryID")]  
        public Categories Categories { get; set; }

    }
}