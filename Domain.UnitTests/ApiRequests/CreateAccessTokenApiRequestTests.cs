using AutoFixture;
using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class CreateAccessTokenApiRequestTests
    {
        private readonly IFixture _fixture;

        public CreateAccessTokenApiRequestTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CreateAccessTokenApiRequest_WhenCalled_ReturnsCreateAccessTokenApiRequest()
        {
            var mockUrl = _fixture.Create<string>();
            var mockData = _fixture.Create<FormUrlEncodedContent>();

            var result = new CreateAccessTokenApiRequest(mockUrl, mockData);

            result.Should().BeOfType<CreateAccessTokenApiRequest>();
            result.PostUrl.Should().Be(mockUrl);
            result.Data.As<CreateAccessTokenApiResponseData>().Request.Should().Be(mockData);
        }
    }
}
