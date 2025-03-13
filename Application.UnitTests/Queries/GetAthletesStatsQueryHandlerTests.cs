using Application.Queries.GetAthletesStats;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Queries
{
    public class GetAthletesStatsQueryHandlerTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAthletesStatsQueryHandler getAthletesStatsQueryHandler;
        private readonly IFixture _fixture;

        public GetAthletesStatsQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockApiClient = _fixture.Freeze<Mock<IApiClient>>();
            _mockMapper = new Mock<IMapper>();
            getAthletesStatsQueryHandler = new GetAthletesStatsQueryHandler(_mockApiClient.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ReturnsGetAthletesStatsQueryResult()
        {
            var getAthletesStatsApiResponse = _fixture.Create<GetAthletesStatsApiResponse>();
            var getAthletesStatsQueryResult = _fixture.Create<GetAthletesStatsQueryResult>();
            _mockApiClient.Setup(x => x.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ReturnsAsync(getAthletesStatsApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthletesStatsQueryResult>(getAthletesStatsApiResponse)).Returns(getAthletesStatsQueryResult);

            var result = await getAthletesStatsQueryHandler.Handle(new GetAthletesStatsQuery(), CancellationToken.None);

            result.Should().BeEquivalentTo(getAthletesStatsQueryResult);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenApiCallFails()
        {
            _mockApiClient.Setup(api => api.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ThrowsAsync(new Exception("Api call failed"));
            var getAthleteStatsQuery = _fixture.Create<GetAthletesStatsQuery>();

            Func<Task> result = async () => await getAthletesStatsQueryHandler.Handle(getAthleteStatsQuery, CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMappingFails()
        {
            var getAthletesStatsApiResponse = _fixture.Create<GetAthletesStatsApiResponse>();
            _mockApiClient.Setup(x => x.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ReturnsAsync(getAthletesStatsApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthletesStatsQueryResult>(getAthletesStatsApiResponse)).Throws(new Exception("Mapping failed"));
            var getAthleteStatsQuery = _fixture.Create<GetAthletesStatsQuery>();

            Func<Task> result = async () => await getAthletesStatsQueryHandler.Handle(getAthleteStatsQuery, CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }
    }
}
