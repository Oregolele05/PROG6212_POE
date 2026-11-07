namespace RaceDay.MVC.ViewModels
{
    public class MyEnrollmentViewModel
    {
        public int EnrollmentID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public int CategoryAge { get; set; }
        public string CategoryDistance { get; set; }
    }
}