using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Models
{
    public class Categories
    {
        [Key]
        public int categoryID { get; set; }
        [Required]
        public int categoryAge { get; set; }
        [Required]
        public string categoryDistance { get; set; }
        //get the foreign key from EventGig
        public int eventID { get; set; }
        [ForeignKey("eventID")]
        public EventGig EventGig { get; set; }
    }
}