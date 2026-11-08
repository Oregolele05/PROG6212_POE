using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApiService _apiService;

        public EnrollmentsController(ApiService apiService) { _apiService = apiService; }

        [HttpPost]
        public async Task<IActionResult> Enroll(int EventID, int CategoryID)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Auth");

            var response = await _apiService.PostAsync("api/enrolments", new { eventID = EventID, categoryID = CategoryID });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Successfully enrolled in the event!";
                return RedirectToAction("MyEnrollments");
            }

            TempData["ErrorMessage"] = "Failed to enrol.";
            return RedirectToAction("Details", "Events", new { id = EventID });
        }

        public async Task<IActionResult> MyEnrollments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Auth");

            var enrolments = await _apiService.GetAsync<IEnumerable<MyEnrollmentViewModel>>("api/enrolments/my-enrolments");
            ViewBag.Enrollments = enrolments ?? new List<MyEnrollmentViewModel>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant") return RedirectToAction("Login", "Auth");

            await _apiService.DeleteAsync($"api/enrolments/{id}");
            TempData["SuccessMessage"] = "Enrolment cancelled.";
            return RedirectToAction("MyEnrollments");
        }

        // GET: /Enrollments/EventRoster/5 (Organiser Only)
        public async Task<IActionResult> EventRoster(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Auth");

            // Fetch the roster from the API
            var roster = await _apiService.GetAsync<IEnumerable<OrganiserRosterViewModel>>($"api/events/{id}/enrolments");

            ViewBag.EventID = id;
            ViewBag.Roster = roster ?? new List<OrganiserRosterViewModel>();

            return View();
        }
    }
}