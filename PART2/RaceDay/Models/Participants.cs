namespace RaceDay.Models
{
    public class Participants
    {
        public int participantID { get; set; }
        public string participantName { get; set; }
        public string participantEmail { get; set; }
        public string participantPhone { get; set; }

        //get the foreign key from Users
        public int userID { get; set; }
        public User User { get; set; }
    }
}
