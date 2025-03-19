using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Reqnroll;

namespace BddTests.StepDefinitions
{
    [Binding]
    [Scope(Feature = "GetAthlete")]
    public sealed class GetAthleteStepDefinitions
    {
        private readonly WireMockServer _server;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private HttpResponseMessage _response;

        public GetAthleteStepDefinitions(ScenarioContext scenarioContext)
        {
            _server = scenarioContext.Get<WireMockServer>();
            _config = scenarioContext.Get<IConfiguration>();
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_config["ApiSettings:BaseUrl"])
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["ApiSettings:AccessToken"]);
        }

        [Given(@"the athlete is authorised")]
        public void GivenTheAthleteIsAuthorised()
        {
            var expectedResponseJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MockResponses", "mockAthleteData.json"));
            _server.Given(Request.Create().WithPath("/api/athlete").UsingGet())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(expectedResponseJson));
        }

        [When(@"I send a GET request to ""(.*)""")]
        public async Task WhenISendAGETRequestTo(string url)
        {
            _response = await _httpClient.GetAsync(url);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }

        [Then(@"the response should contain the athlete's information")]
        public async Task ThenTheResponseShouldContainTheAthletesInformation()
        {
            var contentJson = await _response.Content.ReadAsStringAsync();

            contentJson.Should().Contain("john_doe");
        }
    }
}
