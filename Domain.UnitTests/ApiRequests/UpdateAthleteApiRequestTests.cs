using AutoFixture;
using Domain.Requests;
using FluentAssertions;

namespace Domain.UnitTests.ApiRequests
{
    public class UpdateAthleteApiRequestTests
    {
        private readonly IFixture _fixture;

        public UpdateAthleteApiRequestTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void WhenBuildingThePutRequest_ThenTheRequestIsBuilt()
        {
            var weight = _fixture.Create<float>();

            var result = new UpdateAthleteApiRequest(weight);

            result.PutUrl.Should().BeEquivalentTo("athlete");
            result.Data.Should().BeOfType<UpdateAthleteApiRequestData>();
            result.Data.As<UpdateAthleteApiRequestData>().Weight.Should().Be(weight);
        }
    }
}
