using Application.Queries.GetAthletesStats;
using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using Moq;

namespace Application.UnitTests.Queries
{
    public class GetAthletesStatsQueryHandlerTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAthletesStatsQueryHandler getAthletesStatsQueryHandler;

        public GetAthletesStatsQueryHandlerTests()
        {
            _mockApiClient = new Mock<IApiClient>();
            _mockMapper = new Mock<IMapper>();
            getAthletesStatsQueryHandler = new GetAthletesStatsQueryHandler(_mockApiClient.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ReturnsGetAthletesStatsQueryResult()
        {
            var getAthletesStatsApiResponse = new GetAthletesStatsApiResponse();
            var getAthletesStatsQueryResult = new GetAthletesStatsQueryResult();
            _mockApiClient.Setup(x => x.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ReturnsAsync(getAthletesStatsApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthletesStatsQueryResult>(getAthletesStatsApiResponse)).Returns(getAthletesStatsQueryResult);

            var result = await getAthletesStatsQueryHandler.Handle(new GetAthletesStatsQuery(), CancellationToken.None);

            Assert.Equal(result, getAthletesStatsQueryResult);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenApiCallFails()
        {
            _mockApiClient.Setup(api => api.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ThrowsAsync(new Exception("Api call failed"));
            var query = new GetAthletesStatsQuery();

            await Assert.ThrowsAsync<Exception>(() => getAthletesStatsQueryHandler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMappingFails()
        {
            var getAthletesStatsApiResponse = new GetAthletesStatsApiResponse();
            _mockApiClient.Setup(x => x.Get<GetAthletesStatsApiResponse>(It.IsAny<GetAthletesStatsApiRequest>())).ReturnsAsync(getAthletesStatsApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthletesStatsQueryResult>(getAthletesStatsApiResponse)).Throws(new Exception("Mapping failed"));
            var query = new GetAthletesStatsQuery();

            await Assert.ThrowsAsync<Exception>(() => getAthletesStatsQueryHandler.Handle(query, CancellationToken.None));
        }
    }
}
