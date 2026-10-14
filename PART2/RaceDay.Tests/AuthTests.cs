using System.Net;
using System.Net.Http.Json;
using RaceDay.DTOs;

namespace RaceDay.Tests
{
    public class AuthTests : IClassFixture<RaceDayWebApplicationFactory>
    {
        private readonly RaceDayWebApplicationFactory _factory;

        public AuthTests(RaceDayWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Register_WithValidOrganiserData_Returns201()
        {
            var client = _factory.CreateClient();
            var response = await TestHelpers.RegisterUser(client, "org1@test.com", "Organiser");
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_Returns400()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "dup@test.com");
            var secondResponse = await TestHelpers.RegisterUser(client, "dup@test.com");
            Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_Returns200()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "login1@test.com");
            var response = await TestHelpers.LoginUser(client, "login1@test.com");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithWrongPassword_Returns401()
        {
            var client = _factory.CreateClient();
            await TestHelpers.RegisterUser(client, "wrongpass@test.com");
            var response = await TestHelpers.LoginUser(client, "wrongpass@test.com", "WrongPassword!");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}