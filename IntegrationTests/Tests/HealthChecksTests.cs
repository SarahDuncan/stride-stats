using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Tests;

public class HealthChecksTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public HealthChecksTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var response = await _httpClient.GetAsync("/healthcheck");

        response.EnsureSuccessStatusCode();
    }
}
