using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public int CategoryAge { get; set; }
        public string CategoryDistance { get; set; }
    }

    public class CreateCategoryViewModel
    {
        public int EventID { get; set; }
        [Required][Range(1, 100)] public int CategoryAge { get; set; }
        [Required] public string CategoryDistance { get; set; }
    }
}