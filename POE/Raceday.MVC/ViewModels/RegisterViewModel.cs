using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string Username { get; set; }

        [Required][EmailAddress] public string Email { get; set; }

        [Required][DataType(DataType.Password)] public string Password { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Phone { get; set; }

        [Required] public string Role { get; set; } // "Organiser" or "Participant"
    }
}