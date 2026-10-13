using System.ComponentModel.DataAnnotations;

namespace RaceDay.DTOs
{
    public class Category
    {
        [Required] 
        public int CategoryAge { get; set; }
        [Required] 
        public string CategoryDistance { get; set; }
    }
}
