using Domain.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace IntegrationTests.Tests.Controllers
{
    public class AthleteControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public AthleteControllerTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAthlete_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("/api/athlete");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAthlete_ReturnsExpectedContentType()
        {
            var response = await _httpClient.GetAsync("/api/athlete");

            Assert.Equal("application/json", response?.Content?.Headers?.ContentType?.MediaType);
        }

        [Fact]
        public async Task GetAthlete_ReturnsContent()
        {
            var response = await _httpClient.GetAsync("/api/athlete");

            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task GetAthlete_ReturnsExpectedJson()
        {
            var responseStream = await _httpClient.GetStreamAsync("/api/athlete");

            var response = await JsonSerializer.DeserializeAsync<GetAthleteApiResponse>
                (responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            Assert.NotNull(response?.FirstName);
        }
    }
}
