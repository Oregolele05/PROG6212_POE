using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Models
{
    public class EventGig
    {
      
        public int eventID { get; set; }
      
        public string eventName { get; set; }
        public DateOnly eventDate { get; set; }
        public string eventDescription { get; set; }
        public string eventLocation { get; set; }
        public string eventDistance { get; set; }
        public string eventType { get; set; }
        public string eventRoute { get; set; }
        public string eventMap { get; set; }
        //get the foreign key from Organiser
        public int organiserID { get; set; }
        public Organiser Organiser { get; set; }

    }
}
