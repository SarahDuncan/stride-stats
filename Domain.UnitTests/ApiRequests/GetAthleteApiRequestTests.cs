using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class GetAthleteApiRequestTests
    {
        [Fact]
        public void GetUrl_ReturnsCorrectUrl()
        {
            var getAthleteApiRequest = new GetAthleteApiRequest();

            var getUrl = getAthleteApiRequest.GetUrl;

            getUrl.Should().BeEquivalentTo("athlete");
        }
    }
}
