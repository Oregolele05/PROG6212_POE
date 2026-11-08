using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Phone { get; set; }
    }
}