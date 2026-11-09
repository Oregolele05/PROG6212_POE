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
        // GET: /Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");

            // Fetch the event from the API
            var eventData = await _apiService.GetAsync<Event>($"api/events/{id}");
            if (eventData == null) return NotFound();

            // Map the fetched data to the ViewModel
            var model = new CreateEventViewModel
            {
                EventName = eventData.EventName,
                EventDescription = eventData.EventDescription,
                EventDate = eventData.EventDate,
                EventLocation = eventData.EventLocation,
                EventDistance = eventData.EventDistance,
                EventType = eventData.EventType,
                EventRoute = eventData.EventRoute,
                EventMap = eventData.EventMap
            };

            ViewBag.EventID = id;
            return View(model);
        }

        // POST: /Events/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateEventViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);

            // Send the updated data to the API using PutAsync (JSON)
            var payload = new
            {
                EventName = model.EventName,
                EventDescription = model.EventDescription,
                EventDate = model.EventDate,
                EventLocation = model.EventLocation,
                EventDistance = model.EventDistance,
                EventType = model.EventType,
                EventRoute = model.EventRoute,
                EventMap = model.EventMap
            };

            var response = await _apiService.PutAsync($"api/events/{id}", payload);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {error}");
            return View(model);
        }

        // POST: /Events/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Auth");

            // Call the API to delete the event
            var response = await _apiService.DeleteAsync($"api/events/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Event deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete event. It may have active enrolments or results.";
            }

            return RedirectToAction("Index");
        }
    }
}