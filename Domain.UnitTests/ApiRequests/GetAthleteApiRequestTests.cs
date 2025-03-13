using Domain.Requests;

namespace Domain.UnitTests.ApiRequests
{
    public class GetAthleteApiRequestTests
    {
        [Fact]
        public void GetUrl_ReturnsCorrectUrl()
        {
            var request = new GetAthleteApiRequest();

            var url = request.GetUrl;

            Assert.Equal("athlete", url);
        }
    }
}
