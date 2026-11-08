using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.ViewModels
{
    public class EventViewModel
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public string EventDistance { get; set; }
        public string EventType { get; set; }
        public string? EventRoute { get; set; }
        public string? EventMap { get; set; }

    }

    public class CreateEventViewModel
    {
        [Required] 
        public string EventName { get; set; }
        [Required] 
        public string EventDescription { get; set; }
        [Required]
        [DataType(DataType.Date)] 
        public DateTime EventDate { get; set; }
        [Required] 
        public string EventLocation { get; set; }
        [Required] 
        public string EventDistance { get; set; }
        [Required] 
        public string EventType { get; set; }
        public string? EventRoute { get; set; }
        public string? EventMap { get; set; }
    }
}