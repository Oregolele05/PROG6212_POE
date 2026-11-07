using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService) { _apiService = apiService; }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            var events = await _apiService.GetAsync<IEnumerable<EventViewModel>>("api/events");
            return View(events ?? new List<EventViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            // 1. Fetch the Event
            var eventData = await _apiService.GetAsync<EventViewModel>($"api/events/{id}");
            if (eventData == null) return NotFound();

            // 2. Fetch the Categories separately (Not stored in the EventViewModel)
            var categories = await _apiService.GetAsync<IEnumerable<CategoryViewModel>>($"api/events/{id}/categories");

            ViewBag.Event = eventData;
            ViewBag.Categories = categories ?? new List<CategoryViewModel>();

            return View();
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/events", model);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Failed to create event.");
            return View(model);
        }
    }
}