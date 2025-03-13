using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class GetAthleteStatsApiRequestTests
    {
        [Fact]
        public void GetUrl_ReturnsCorrectUrl()
        {
            var athleteId = 123;
            var request = new GetAthletesStatsApiRequest(athleteId);

            var url = request.GetUrl;

            url.Should().BeEquivalentTo($"athletes/{athleteId}/stats");
        }
    }
}
