using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using RaceDay.MVC.ViewModels;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/auth/login", new
            {
                UserEmail = model.Email,
                UserPassword = model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                // Read the JSON response from the API
                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                // Store in MVC Session
                HttpContext.Session.SetString("UserRole", root.GetProperty("role").GetString()!);
                HttpContext.Session.SetInt32("UserID", root.GetProperty("userID").GetInt32());
                HttpContext.Session.SetString("UserEmail", model.Email);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiService.PostAsync("api/auth/register", new
            {
                Username = model.Username,
                UserEmail = model.Email,
                UserPassword = model.Password,
                UserRole = model.Role,
                Name = model.Name,
                Phone = model.Phone
            });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {errorContent}");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}