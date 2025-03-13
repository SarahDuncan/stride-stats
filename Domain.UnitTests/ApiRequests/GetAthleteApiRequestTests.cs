using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class GetAthleteApiRequestTests
    {
        [Fact]
        public void GetUrl_ReturnsCorrectUrl()
        {
            var request = new GetAthleteApiRequest();

            var url = request.GetUrl;

            url.Should().BeEquivalentTo("athlete");
        }
    }
}
