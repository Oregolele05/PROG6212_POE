namespace RaceDay.MVC.ViewModels
{
    public class OrganiserRosterViewModel
    {
        public int EnrollmentID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantEmail { get; set; }
        public int CategoryAge { get; set; }
        public string CategoryDistance { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}