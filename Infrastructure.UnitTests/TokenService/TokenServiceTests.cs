using AutoFixture;
using AutoFixture.AutoMoq;
using Domain.Interfaces.Api;
using Domain.Responses;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Infrastructure.UnitTests.TokenService
{
    public class TokenServiceTests
    {
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly IFixture _fixture;
        private readonly Mock<Cache.TokenService> _tokenServiceMock;

        public TokenServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockMemoryCache = _fixture.Freeze<Mock<IMemoryCache>>();
            _mockConfiguration = _fixture.Freeze<Mock<IConfiguration>>();
            _mockApiClient = _fixture.Freeze<Mock<IApiClient>>();

            _mockConfiguration.Setup(x => x["ApiSettings:ClientId"]).Returns("mock-client-id");
            _mockConfiguration.Setup(x => x["ApiSettings:ClientSecret"]).Returns("mock-client-secret");
            _mockConfiguration.Setup(x => x["ApiSettings:TokenUrl"]).Returns("http://localhost:5000/token");


            _tokenServiceMock = new Mock<Cache.TokenService>(_mockMemoryCache.Object, _mockConfiguration.Object, _mockApiClient.Object);
            _tokenServiceMock.CallBase = true;
        }

        [Fact]
        public async Task GetAccessTokenAsync_ReturnsToken_WhenAccessTokenExistsInCache()
        {
            var mockAccessToken = _fixture.Create<string>();
            object outValue = mockAccessToken;
            _mockMemoryCache.Setup(x => x.TryGetValue("AccessToken", out outValue)).Returns(true);

            var result = await _tokenServiceMock.Object.GetTokenAsync();

            result.Should().Be(mockAccessToken);
            _tokenServiceMock.Verify(x => x.GetRefreshTokenAsync(), Times.Never());
        }

        [Fact]
        public async Task GetAccessTokenAsync_GetsNewAccessToken_WhenAccessTokenDoesNotExistInCache_AndRefreshTokenExistsInCache()
        {
            var mockAccessToken = _fixture.Create<string>();
            object mockAccessTokenOutValue = mockAccessToken;
            var mockRefreshToken = _fixture.Create<string>();
            object mockRefreshTokenOutValue = mockRefreshToken;
            _mockMemoryCache.Setup(x => x.TryGetValue("AccessToken", out mockAccessTokenOutValue)).Returns(false);
            _mockMemoryCache.Setup(x => x.TryGetValue("RefreshToken", out mockRefreshTokenOutValue)).Returns(true);
            _mockApiClient.Setup(x => x.Post<CreateAccessTokenApiResponse>(It.IsAny<Domain.Requests.CreateAccessTokenApiRequest>()))
                .ReturnsAsync(new CreateAccessTokenApiResponse { AccessToken = mockAccessToken });

            var result = await _tokenServiceMock.Object.GetTokenAsync();

            result.Should().Be(mockAccessToken);
            _tokenServiceMock.Verify(x => x.GetRefreshTokenAsync(), Times.Once());
        }

        [Fact]
        public async Task GetRefreshTokenAsync_SuccessfullyRetrievesTheRefreshToken_WhenTheRefreshTokenExistsInCache()
        {
            var mockAccessToken = _fixture.Create<string>();
            object mockAccessTokenOutValue = mockAccessToken;
            var mockRefreshToken = _fixture.Create<string>();
            object mockRefreshTokenOutValue = mockRefreshToken;
            _mockMemoryCache.Setup(x => x.TryGetValue("RefreshToken", out mockRefreshTokenOutValue)).Returns(true);
            _mockApiClient.Setup(x => x.Post<CreateAccessTokenApiResponse>(It.IsAny<Domain.Requests.CreateAccessTokenApiRequest>()))
                .ReturnsAsync(new CreateAccessTokenApiResponse { AccessToken = mockAccessToken });

            var result = await _tokenServiceMock.Object.GetRefreshTokenAsync();

            result.Should().Be(mockAccessToken);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_ThrowsUnauthorisedAccessException_WhenThereIsNoRefreshTokenCached()
        {
            string mockRefreshToken = null;
            object mockRefreshTokenObject = mockRefreshToken;
            _mockMemoryCache.Setup(x => x.TryGetValue("RefreshToken", out mockRefreshTokenObject!)).Returns(false);

            Func<Task> result = _tokenServiceMock.Object.GetRefreshTokenAsync;

            var exception = await result.Should().ThrowAsync<UnauthorizedAccessException>();
            exception.Which.Message.Should().Be("Refresh token not found.");
        }
    }
}
