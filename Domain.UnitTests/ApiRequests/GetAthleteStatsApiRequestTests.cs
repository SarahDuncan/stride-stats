using Domain.Requests;

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

            Assert.Equal($"athletes/{athleteId}/stats", url);
        }
    }
}
