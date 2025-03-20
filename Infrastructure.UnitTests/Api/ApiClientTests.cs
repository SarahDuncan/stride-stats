using Moq;
using Microsoft.Extensions.Configuration;
using Application.Api;
using Domain.Responses;
using System.Text.Json;
using System.Net;
using Domain.Requests;
using Moq.Protected;
using FluentAssertions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Interfaces.Cache;

namespace Infrastructure.UnitTests.Api
{
    public class ApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly ApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly IFixture _fixture;
        private readonly Mock<ITokenService> _tokenService;

        public ApiClientTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _mockHttpMessageHandler =  _fixture.Freeze<Mock<HttpMessageHandler>>();
            _mockConfiguration = _fixture.Freeze<Mock<IConfiguration>>();
            _tokenService = _fixture.Freeze<Mock<ITokenService>>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockConfiguration.Setup(x => x["ApiSettings:BaseUrl"]).Returns("http://localhost:5000");
            _tokenService.Setup(x => x.GetTokenAsync()).ReturnsAsync("mock-token");
            _apiClient = new ApiClient(_httpClient, _mockConfiguration.Object, _tokenService.Object);
        }

        [Fact]
        public async Task Get_ReturnsDeserialisedResponse_WhenApiCallIsSuccessful()
        {
            var apiRequest = _fixture.Create<GetAthleteApiRequest>();
            var apiResponse = _fixture.Create<GetAthleteApiResponse>();
            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResponse) };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var result = await _apiClient.Get<GetAthleteApiResponse>(apiRequest);

            result.Id.Should().Be(apiResponse.Id);
        }

        [Fact]
        public async Task Get_ThrowsException_WhenHttpRequestFails()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));
            var getAthleteApiRequest =_fixture.Create<GetAthleteApiRequest>();

            Func<Task> result = async () => await _apiClient.Get<GetAthleteApiResponse>(getAthleteApiRequest);

            var exception = await result.Should().ThrowAsync<Exception>();
            exception.Which.InnerException.Should().NotBeNull();
            exception.Which.InnerException.Message.Should().Be("Request failed");
            exception.Which.Message.Should().Be($"HTTP request failed, exception: {exception.Which.InnerException?.Message}", exception.Which.Message);
        }

        [Fact]
        public async Task Get_ThrowsException_WhenJsonDeserialisationFails()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("invalid json") };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);
            var getAthleteApiRequest = _fixture.Create<GetAthleteApiRequest>();

            Func<Task> result = async () => await _apiClient.Get<GetAthleteApiResponse>(getAthleteApiRequest);

            var exception = await result.Should().ThrowAsync<Exception>();
            exception.Which.InnerException.Should().NotBeNull();
            exception.Which.Message.Should().Be($"JSON deserialization failed, exception: {exception.Which.InnerException?.Message}", exception.Which.Message);
        }
    }
}
