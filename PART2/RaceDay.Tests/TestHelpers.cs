using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RaceDay.DTOs;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;

namespace RaceDay.Tests
{
    public static class TestHelpers
    {
        public static async Task<HttpResponseMessage> RegisterUser(
            HttpClient client, string email, string role = "Participant")
        {
            // CHANGED: 'RegisterDto' to 'Registration'
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
            // CHANGED: 'LoginDto' to 'Login'
            var dto = new Login { UserEmail = email, UserPassword = password };
            return await client.PostAsJsonAsync("/api/auth/login", dto);
        }

        public static async Task<HttpClient> CreateLoggedInClient(
            RaceDayWebApplicationFactory factory, string email, string role)
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true
            });

            await RegisterUser(client, email, role);
            await LoginUser(client, email);
            return client;
        }

        // This uses 'Event' (which you confirmed is the correct name)
        public static MultipartFormDataContent CreateEventFormData(Event dto)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(dto.EventName ?? ""), "EventName");
            formData.Add(new StringContent(dto.EventDescription ?? ""), "EventDescription");
            formData.Add(new StringContent(dto.EventDate.ToString("yyyy-MM-dd")), "EventDate");
            formData.Add(new StringContent(dto.EventLocation ?? ""), "EventLocation");
            formData.Add(new StringContent(dto.EventDistance ?? ""), "EventDistance");
            formData.Add(new StringContent(dto.EventType ?? ""), "EventType");

            if (dto.EventRoute != null) formData.Add(new StringContent(dto.EventRoute), "EventRoute");
            if (dto.EventMap != null) formData.Add(new StringContent(dto.EventMap), "EventMap");

            return formData;
        }
    }
}