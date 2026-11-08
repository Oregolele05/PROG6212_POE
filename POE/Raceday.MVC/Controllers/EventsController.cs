using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            var events = await _apiService.GetAsync<IEnumerable<EventViewModel>>("api/events");
            return View(events ?? new List<EventViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventData = await _apiService.GetAsync<EventViewModel>($"api/events/{id}");
            if (eventData == null) return NotFound();

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

            // Build Multipart Form Data
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(model.EventName ?? ""), "EventName");
            formData.Add(new StringContent(model.EventDescription ?? ""), "EventDescription");
            formData.Add(new StringContent(model.EventDate.ToString("yyyy-MM-dd")), "EventDate");
            formData.Add(new StringContent(model.EventLocation ?? ""), "EventLocation");
            formData.Add(new StringContent(model.EventDistance ?? ""), "EventDistance");
            formData.Add(new StringContent(model.EventType ?? ""), "EventType");

            if (model.EventRoute != null) formData.Add(new StringContent(model.EventRoute), "EventRoute");
            if (model.EventMap != null) formData.Add(new StringContent(model.EventMap), "EventMap");

            // Add the image file if the user selected one
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileStream = new StreamContent(model.ImageFile.OpenReadStream());
                fileStream.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.ImageFile.ContentType);
                formData.Add(fileStream, "imageFile", model.ImageFile.FileName);
            }

            var response = await _apiService.PostFormAsync("api/events", formData);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction("Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {errorContent}");
            return View(model);
        }
    }
}