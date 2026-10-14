using System.Net;
using System.Net.Http.Json;
using RaceDay.DTOs;

namespace RaceDay.Tests
{
    public class RoleAuthorizationTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public RoleAuthorizationTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task CreateEvent_AsParticipant_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "p1@test.com", "Participant");
            var eventDto = new Event
            {
                EventName = "Test Event",
                EventDescription = "Desc",
                EventDate = DateTime.Now.AddDays(7),
                EventLocation = "Cape Town",
                EventDistance = "10KM",
                EventType = "Run"
            };
            var response = await client.PostAsJsonAsync("/api/events", eventDto);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_Unauthenticated_Returns401()
        {
            var client = _factory.CreateClient();
            var eventDto = new Event
            {
                EventName = "Test Event",
                EventDescription = "Desc",
                EventDate = DateTime.Now.AddDays(7),
                EventLocation = "Cape Town",
                EventDistance = "10KM",
                EventType = "Run"
            };
            var response = await client.PostAsJsonAsync("/api/events", eventDto);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Enroll_AsOrganiser_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "org2@test.com", "Organiser");
            var enrollDto = new Enrollment { EventID = 1, CategoryID = 1 };
            var response = await client.PostAsJsonAsync("/api/enrolments", enrollDto);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}