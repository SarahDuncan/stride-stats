using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Reqnroll;
using System.Net;
using System.Net.Http.Headers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace BddTests.StepDefinitions
{
    [Binding]
    [Scope(Feature = "GetAthletesStats")]
    public sealed class GetAthletesStatsStepDefinitions
    {
        private readonly WireMockServer _server;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private HttpResponseMessage _response;
        private readonly IFixture _fixture;
        private readonly long _athleteId;

        public GetAthletesStatsStepDefinitions(ScenarioContext scenarioContext)
        {
            _server = scenarioContext.Get<WireMockServer>();
            _config = scenarioContext.Get<IConfiguration>();
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_config["ApiSettings:BaseUrl"])
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["ApiSettings:AccessToken"]);
            _fixture = new Fixture();
            _athleteId = _fixture.Create<long>();
        }

        [Given(@"the athlete is authorised")]
        public void GivenTheAthleteIsAuthorised()
        {
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthletesStatsData.json"));
            _server.Given(Request.Create().WithPath($"/api/athlete/{_athleteId}/stats").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(expectedResponseJson));
        }

        [Given(@"the athlete is not authorised")]
        public void GivenTheAthleteIsNotAuthorised()
        {
            _server.Given(Request.Create().WithPath($"/api/athlete/{_athleteId}/stats").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(401));
        }

        [When(@"I send a GET request to ""(.*)""")]
        public async Task WhenISendAGETRequestTo(string url)
        {
            url = url.Replace("{athleteId}", _athleteId.ToString());
            _response = await _httpClient.GetAsync(url);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"the response body should be a JSON object containing the athlete's stats")]
        public async Task ThenTheResponseBodyShouldBeAJsonObjectContainingTheAthletesStats()
        {
            var contentJson = await _response.Content.ReadAsStringAsync();

            contentJson.Should().Contain("6311.74");
        }
    }
}
