using System.Net;
using System.Net.Http.Json;
using RaceDay.DTOs;
using Xunit;

namespace RaceDay.Tests
{
    public class EventsTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public EventsTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task CreateEvent_AsOrganiser_Returns201()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "orgEvent@test.com", "Organiser");

            var eventDto = new Event
            {
                EventName = "Cape Town Marathon",
                EventDescription = "Annual marathon",
                EventDate = DateTime.Now.AddDays(14),  // ← DateTime now
                EventLocation = "Cape Town",
                EventDistance = "42KM",
                EventType = "Run"
            };

            var response = await client.PostAsJsonAsync("/api/events", eventDto);
            var body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Expected 201 but got {(int)response.StatusCode}. Body: {body}");
            }
        }

        [Fact]
        public async Task GetAllEvents_PublicAccess_Returns200()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/events");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}