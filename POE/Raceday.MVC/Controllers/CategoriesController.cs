using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApiService _apiService;

        public CategoriesController(ApiService apiService) { _apiService = apiService; }

        public async Task<IActionResult> Index(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");

            var eventData = await _apiService.GetAsync<EventViewModel>($"api/events/{id}");
            if (eventData == null) return NotFound();

            var categories = await _apiService.GetAsync<IEnumerable<CategoryViewModel>>($"api/events/{id}/categories");

            ViewBag.EventName = eventData.EventName;
            ViewBag.EventID = id;
            ViewBag.Categories = categories ?? new List<CategoryViewModel>();

            return View();
        }

        public IActionResult Create(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            return View(new CreateCategoryViewModel { EventID = id });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser") return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync($"api/events/{model.EventID}/categories", model);
            if (response.IsSuccessStatusCode) return RedirectToAction("Index", new { id = model.EventID });

            ModelState.AddModelError("", "Failed to create category.");
            return View(model);
        }
    }
}