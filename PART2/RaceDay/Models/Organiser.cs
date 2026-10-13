namespace RaceDay.Models
{
    public class Organiser
    {
        public int organiserID { get; set; }
        public string organiserName { get; set; }
        public string organiserEmail { get; set; }
        public string organiserPhone { get; set; }

        //get the foreign key from Users
        public int userID { get; set; }
        public User User { get; set; }
    }
}