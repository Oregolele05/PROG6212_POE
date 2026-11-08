using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;

namespace RaceDay.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApiService _apiService;

        public UsersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("UserRole") == null) return RedirectToAction("Login", "Auth");

            // Fetch profile from API
            var profile = await _apiService.GetAsync<UserProfileViewModel>("api/users/profile");
            if (profile == null) return NotFound();

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") == null) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PutAsync("api/users/profile", new
            {
                Username = model.Username,
                UserEmail = model.UserEmail,
                Name = model.Name,
                Phone = model.Phone
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {error}");
            return View(model);
        }
    }
}