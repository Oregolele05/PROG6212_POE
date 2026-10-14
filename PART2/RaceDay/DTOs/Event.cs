using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class Event
    {
        [Required] public string EventName { get; set; }
        [Required] public string EventDescription { get; set; }
        [Required] public DateTime EventDate { get; set; }
        [Required] public string EventLocation { get; set; }
        [Required] public string EventDistance { get; set; }
        [Required] public string EventType { get; set; } // Run, Walk, Cycle
        public string? EventRoute { get; set; }
        public string? EventMap { get; set; }
    }
}
