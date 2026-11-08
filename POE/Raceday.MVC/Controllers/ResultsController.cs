using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApiService _apiService;

        public ResultsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Results/Capture/5 (Organiser only)
        public IActionResult Capture(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Auth");

            return View(new CaptureResultViewModel { EventID = id });
        }

        // POST: /Results/Capture
        [HttpPost]
        public async Task<IActionResult> Capture(CaptureResultViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            // API expects time format like "00:52:35" which is TimeSpan.ToString() by default in JSON
            var payload = new
            {
                ParticipantID = model.ParticipantID,
                ParticipantTime = model.ParticipantTime,
                ParticipantPosition = model.ParticipantPosition
            };

            var response = await _apiService.PostAsync($"api/events/{model.EventID}/results", payload);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Result captured successfully!";
                return RedirectToAction("Index", "Events"); // Send back to dashboard
            }

            ModelState.AddModelError("", "Failed to capture result. Check if the participant is enrolled.");
            return View(model);
        }

        // GET: /Results/MyResults (Participant only)
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Auth");

            var results = await _apiService.GetAsync<IEnumerable<MyResultViewModel>>("api/results/my-results");
            ViewBag.Results = results ?? new List<MyResultViewModel>();

            return View();
        }

        // GET: /Results/Leaderboard/5 (Public)
        public async Task<IActionResult> Leaderboard(int id)
        {
            var leaderboard = await _apiService.GetAsync<IEnumerable<LeaderboardViewModel>>($"api/events/{id}/results");

            ViewBag.Leaderboard = leaderboard ?? new List<LeaderboardViewModel>();
            ViewBag.EventID = id;

            return View();
        }
    }
}