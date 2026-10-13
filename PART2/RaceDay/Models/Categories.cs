namespace RaceDay.Models
{
    public class Categories
    {
        public int categoryID { get; set; }
        public int categoryAge { get; set; }
        public string categoryDistance { get; set; }
        //get the foreign key from EventGig
        public int eventID { get; set; }
        public EventGig EventGig { get; set; }
    }
}