using AutoFixture;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace IntegrationTests.Tests.Controllers
{
    public class AthleteControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly WireMockServer _wireMockServer;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly IFixture _fixture;

        public AthleteControllerTests(WebApplicationFactory<Program> factory)
        {
            _wireMockServer = WireMockServer.Start();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>()
            {
                {"ApiSettings:BaseUrl", _wireMockServer.Urls[0] },
                {"ApiSettings:AccessToken", "mock_token" }
            });
                });
            });

            _httpClient = _factory.CreateClient();
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAthlete_ReturnsSuccessStatusCode()
        {
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthleteData.json"));
            _wireMockServer.Given(Request.Create()
                .WithPath("/athlete")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(expectedResponseJson));

            var response = await _httpClient.GetAsync("/api/athlete");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAthlete_ReturnsExpectedResponse()
        {
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthleteData.json"));
            var expectedResponse = JsonSerializer.Deserialize<GetAthleteApiResponse>(expectedResponseJson, jsonOptions);
            _wireMockServer.Given(Request.Create().WithPath("/athlete").UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(expectedResponseJson));
            var response = await _httpClient.GetAsync("/api/athlete");
            var content = await response.Content.ReadAsStringAsync();
            var model = await response.Content.ReadFromJsonAsync<GetAthleteApiResponse>(jsonOptions);

            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.Equal(expectedResponse?.FirstName, model.FirstName);
            });
        }

        [Fact]
        public async Task GetAthletesStats_ReturnsSuccessStatusCode()
        {
            var athleteId = _fixture.Create<long>();
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthletesStatsData.json"));
            _wireMockServer.Given(Request.Create()
                .WithPath($"/athletes/{athleteId}/stats")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(expectedResponseJson));

            var response = await _httpClient.GetAsync($"/api/athlete/{athleteId}/stats");
            Console.WriteLine(response.RequestMessage.RequestUri);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAthletesStats_ReturnsExpectedResponse()
        {
            var athleteId = _fixture.Create<long>();
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthletesStatsData.json"));
            var expectedResponse = JsonSerializer.Deserialize<GetAthletesStatsApiResponse>(expectedResponseJson, jsonOptions);
            _wireMockServer.Given(Request.Create()
                .WithPath($"/athletes/{athleteId}/stats")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(expectedResponseJson));

            var response = await _httpClient.GetAsync($"/api/athlete/{athleteId}/stats");
            var content = await response.Content.ReadAsStringAsync();
            var model = await response.Content.ReadFromJsonAsync<GetAthletesStatsApiResponse>(jsonOptions);

            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.Equal(expectedResponse?.AllRunTotals, model.AllRunTotals);
            });
        }
    }
}
