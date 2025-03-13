using AutoFixture;
using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class GetAthletesStatsApiRequestTests
    {
        private readonly IFixture _fixture;

        public GetAthletesStatsApiRequestTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GetUrl_ReturnsCorrectUrl()
        {
            var athleteId = _fixture.Create<long>();
            var getAthletesStatsApiRequest = new GetAthletesStatsApiRequest(athleteId);

            var getUrl = getAthletesStatsApiRequest.GetUrl;

            getUrl.Should().BeEquivalentTo($"athletes/{athleteId}/stats");
        }
    }
}
