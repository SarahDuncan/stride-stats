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
            _httpClient = webApplicationFactory.CreateDefaultClient(new Uri("https://localhost/api/athlete"));
        }

        [Fact]
        public async Task GetAthlete_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAthlete_ReturnsExpectedResponse()
        {
            var expectedResponse = JsonSerializer.Deserialize<GetAthleteApiResponse>
                (File.ReadAllText("MockResponses/GetAthleteApiResponse.json"));

            var model = await _httpClient.GetFromJsonAsync<GetAthleteApiResponse>("");

            Assert.Equal(expectedResponse, model);
            Assert.NotNull(model?.FirstName);
        }
    }
}
