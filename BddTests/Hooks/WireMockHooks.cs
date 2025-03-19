using Microsoft.Extensions.Configuration;
using Reqnroll;
using WireMock.Server;

namespace BddTests.Hooks
{
    [Binding]
    public class WireMockHooks
    {
        private WireMockServer _server;
        private readonly ScenarioContext _scenarioContext;
        private IConfiguration _config;

        public WireMockHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void StartWireMockServer()
        {
            _server = WireMockServer.Start();

            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    {"ApiSettings:BaseUrl", _server.Urls[0] },
                    {"ApiSettings:AccessToken", "mock_token" }
                })
                .Build();
            _config = configBuilder;

            _scenarioContext.Set(_server);
            _scenarioContext.Set(_config);
        }

        [AfterScenario]
        public void StopWireMockServer()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
