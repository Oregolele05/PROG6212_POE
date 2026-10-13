using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class EventGig
    {
        [Key]
        public int eventID { get; set; }
        [Required]
        public string eventName { get; set; }
        [Required]
        public DateOnly eventDate { get; set; }
        [Required]
        public string eventDescription { get; set; }
        [Required]
        public string eventLocation { get; set; }
        [Required]
        public string eventDistance { get; set; }
        [Required]
        public string eventType { get; set; }
        [Required]
        public string eventRoute { get; set; }
        [Required]
        public string eventMap { get; set; }
        //get the foreign key from Organiser
        public int organiserID { get; set; }
        [ForeignKey("organiserID")]
        public Organiser Organiser { get; set; }
    }

}