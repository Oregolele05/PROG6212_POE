namespace RaceDay.Models
{
    public class Results
    {
        public int resultID { get; set; }
        public TimeOnly participantTime { get; set; }
        public string participantPosition { get; set; }
        
        // get the foreign key from events
        public int eventID { get; set; }
        public EventGig EventGig { get; set; }

        // get the foreign key from participants
        public int participantID { get; set; }
        public Participants Participants { get; set; }
    }
}
