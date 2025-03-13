using Moq;
using Microsoft.Extensions.Configuration;
using Application.Api;
using Domain.Responses;
using System.Text.Json;
using System.Net;
using Domain.Requests;
using Moq.Protected;

namespace Infrastructure.UnitTests.Api
{
    public class ApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly ApiClient _apiClient;
        private readonly HttpClient _httpClient;

        public ApiClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration = new Mock<IConfiguration>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockConfiguration.Setup(x => x["ApiSettings:BaseUrl"]).Returns("http://localhost:5000");
            _apiClient = new ApiClient(_httpClient, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Get_ReturnsDeserialisedResponse_WhenApiCallIsSuccessful()
        {
            var apiResponse = new GetAthleteApiResponse { Id = 1 };
            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResponse) };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);
            var request = new GetAthleteApiResponse();

            var result = await _apiClient.Get<GetAthleteApiResponse>(new GetAthleteApiRequest());

            Assert.Equal(apiResponse.Id, result.Id);
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
            var request = new GetAthleteApiResponse();

            var exception = await Assert.ThrowsAsync<Exception>(() => _apiClient.Get<GetAthleteApiResponse>(new GetAthleteApiRequest()));

            Assert.Equal($"HTTP request failed, exception: {exception.InnerException?.Message}", exception.Message);
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
            var request = new GetAthleteApiResponse();

            var exception = await Assert.ThrowsAsync<Exception>(() => _apiClient.Get<GetAthleteApiResponse>(new GetAthleteApiRequest()));
            Assert.Equal($"JSON deserialization failed, exception: {exception.InnerException!.Message}", exception.Message);
        }
    }
}
