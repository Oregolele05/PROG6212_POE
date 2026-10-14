using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class ResultDTO
    {
        [Required] 
        public int ParticipantID { get; set; }
        [Required] 
        public TimeSpan ParticipantTime { get; set; }
        [Required] 
        public string ParticipantPosition { get; set; }
    }
}
