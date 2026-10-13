namespace RaceDay.Models
{
    public class EventEnrollment
    {
        public int enrollmentID { get; set; }
        public DateOnly enrollmentDate { get; set; }

        //get the foreign key from EventGig
        public int eventID { get; set; }
        public EventGig EventGig { get; set; }

        //get foreign key from Participant
        public int participantID { get; set; }
        public Participant Participants { get; set; }

        //get foreign key from Categories
        public int categoryID { get; set; }
        public Categories Categories { get; set; }

    }
}