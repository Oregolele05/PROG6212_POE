using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RaceDay.DTOs;

namespace RaceDay.Tests
{
    public static class TestHelpers
    {
        public static async Task<HttpResponseMessage> RegisterUser(
            HttpClient client, string email, string role = "Participant")
        {
            var dto = new Registration
            {
                Username = email.Split('@')[0],
                UserEmail = email,
                UserPassword = "Password123!",
                UserRole = role,
                Name = "Test User",
                Phone = "0825550000"
            };
            return await client.PostAsJsonAsync("/api/auth/register", dto);
        }

        public static async Task<HttpResponseMessage> LoginUser(
            HttpClient client, string email, string password = "Password123!")
        {
            var dto = new Login { UserEmail = email, UserPassword = password };
            return await client.PostAsJsonAsync("/api/auth/login", dto);
        }

        public static async Task<HttpClient> CreateLoggedInClient(
            RaceDayWebApplicationFactory factory, string email, string role)
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true // ← Must be true, otherwise session cookie is dropped
            });

            var regResponse = await RegisterUser(client, email, role);
            if (!regResponse.IsSuccessStatusCode)
            {
                var body = await regResponse.Content.ReadAsStringAsync();
                throw new Exception($"Registration failed with {(int)regResponse.StatusCode}. Body: {body}");
            }

            var loginResponse = await LoginUser(client, email);
            if (!loginResponse.IsSuccessStatusCode)
            {
                var body = await loginResponse.Content.ReadAsStringAsync();
                throw new Exception($"Login failed with {(int)loginResponse.StatusCode}. Body: {body}");
            }

            return client;
        }
    }
}