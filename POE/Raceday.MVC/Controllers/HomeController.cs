using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService) { _apiService = apiService; }

        public async Task<IActionResult> Index()
        {
            var events = await _apiService.GetAsync<IEnumerable<EventViewModel>>("api/events");
            return View(events ?? new List<EventViewModel>());
        }
    }
}