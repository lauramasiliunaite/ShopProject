using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using TestShop.Tests.Utils;
using TestShop.Application.DTOs.Auth;

namespace TestShop.Tests
{
    public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Products_Get_Should_Return_Ok_And_Items()
        {
            var resp = await _client.GetAsync("/api/products?page=1&pageSize=5");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = await resp.Content.ReadAsStringAsync();
            json.Should().Contain("\"success\":true");
            json.Should().Contain("\"items\"");
        }

        [Fact]
        public async Task Auth_Login_Invalid_Should_Return_401()
        {
            var body = new LoginRequest("demo", "wrong");
            var resp = await _client.PostAsJsonAsync("/api/auth/login", body);
            resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var text = await resp.Content.ReadAsStringAsync();
            text.Should().Contain("UNAUTHORIZED");
        }
    }
}
