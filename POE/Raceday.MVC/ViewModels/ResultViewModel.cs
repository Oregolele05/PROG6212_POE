using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.ViewModels
{
    // Used by Organiser to capture a result
    public class CaptureResultViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Participant ID is required")]
        public int ParticipantID { get; set; }

        [Required(ErrorMessage = "Finish time is required")]
        public TimeSpan ParticipantTime { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public string ParticipantPosition { get; set; }
    }

    // Used by Participant to view their history
    public class MyResultViewModel
    {
        public int ResultID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public string ParticipantPosition { get; set; }
        public TimeSpan ParticipantTime { get; set; }
    }

    // Used for the Public Leaderboard
    public class LeaderboardViewModel
    {
        public string ParticipantName { get; set; }
        public string ParticipantPosition { get; set; }
        public TimeSpan ParticipantTime { get; set; }
    }
}