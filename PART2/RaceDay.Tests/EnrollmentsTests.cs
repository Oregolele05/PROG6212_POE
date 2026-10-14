using System.Net;
using System.Net.Http.Json;
using RaceDay.DTOs;

namespace RaceDay.Tests
{
    public class EnrollmentsTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public EnrollmentsTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Enroll_WithoutEventOrCategory_Returns404OrBadRequest()
        {
            // This test verifies the endpoint validates references
            var client = await TestHelpers.CreateLoggedInClient(_factory, "enrol@test.com", "Participant");
            var enrollDto = new Enrollment { EventID = 9999, CategoryID = 9999 };
            var response = await client.PostAsJsonAsync("/api/enrolments", enrollDto);
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.BadRequest,
                $"Expected 404 or 400 but got {(int)response.StatusCode}");
        }

        [Fact]
        public async Task GetMyEnrollments_AsParticipant_Returns200()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "myenrol@test.com", "Participant");
            var response = await client.GetAsync("/api/enrolments/my-enrolments");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetEventRoster_AsParticipant_Returns403()
        {
            var client = await TestHelpers.CreateLoggedInClient(_factory, "roster@test.com", "Participant");
            var response = await client.GetAsync("/api/events/1/enrolments");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}