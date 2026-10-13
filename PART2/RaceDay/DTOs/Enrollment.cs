using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class Enrollment
    {
        [Required]
        public int EventID { get; set; }
        [Required] 
        public int CategoryID { get; set; }
    }
}
