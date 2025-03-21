using AutoFixture;
using Domain.Interfaces.Cache;
using Domain.Requests;
using Domain.Responses;
using IntegrationTests.MockServices;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using WireMock.Matchers;
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
                {"ApiSettings:ClientId", "mock_client_id" },
                {"ApiSettings:ClientSecret", "mock_client_secret" },
                {"ApiSettings:RedirectUri", "https://mock.redirecturi.com" },
                {"ApiSettings:AuthUrl", "https://mock.authurl.com" },
                {"ApiSettings:TokenUrl", "https://mock.tokenurl.com" }
            });
                });

                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ITokenService));
                    if (descriptor is not null) services.Remove(descriptor);
                    services.AddSingleton<ITokenService, MockTokenService>();
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
            _wireMockServer.Given(Request.Create()
                .WithPath("/athlete")
                .UsingGet())
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

        [Fact]
        public async Task UpdateAthlete_ReturnsSuccessStatusCode()
        {
            var putContent = _fixture.Create<UpdateAthleteApiRequestData>();
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthleteData.json"));
            _wireMockServer.Given(Request.Create()
                .WithPath("/athlete")
                .WithBody(new JsonMatcher(JsonSerializer.Serialize(putContent)))
                .UsingPut())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(expectedResponseJson));

            var response = await _httpClient.PutAsJsonAsync($"/api/athlete/{putContent.Weight}", putContent);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateAthlete_ReturnsExpectedResponse()
        {
            var putContent = _fixture.Create<UpdateAthleteApiRequestData>();
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthleteData.json"));
            var expectedResponse = JsonSerializer.Deserialize<UpdateAthleteApiResponse>(expectedResponseJson, jsonOptions);
            _wireMockServer.Given(Request.Create()
                .WithPath("/athlete")
                .WithBody(new JsonMatcher(JsonSerializer.Serialize(putContent)))
                .UsingPut())
                .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(expectedResponseJson));

            var response = await _httpClient.PutAsJsonAsync($"api/athlete/{putContent.Weight}", putContent);
            var content = await response.Content.ReadAsStringAsync();
            var model = await response.Content.ReadFromJsonAsync<UpdateAthleteApiResponse>(jsonOptions);

            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.Equal(expectedResponse?.FirstName, model.FirstName);
            });
        }
    }
}
