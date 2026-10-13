namespace RaceDay.DTOs
{
    public class Register
    {
        public string username { get; set; }  
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string userRole { get; set; }

        //these will be used to populate Organizer and Participant tables

        public string name { get; set; }    
        public string phone { get; set; }

    }
}
