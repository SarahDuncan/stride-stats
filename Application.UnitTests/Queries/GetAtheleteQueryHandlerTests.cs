using Application.Queries.GetAthlete;
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
    public class GetAtheleteQueryHandlerTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAthleteQueryHandler getAthleteQueryHandler;
        private readonly IFixture _fixture;

        public GetAtheleteQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockApiClient = _fixture.Freeze<Mock<IApiClient>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();
            getAthleteQueryHandler = new GetAthleteQueryHandler(_mockApiClient.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ReturnsGetAthleteQueryResult()
        {
            var getAthleteApiResponse = _fixture.Create<GetAthleteApiResponse>();
            var getAthleteQueryResult = _fixture.Create<GetAthleteQueryResult>();
            _mockApiClient.Setup(x => x.Get<GetAthleteApiResponse>(It.IsAny<GetAthleteApiRequest>())).ReturnsAsync(getAthleteApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthleteQueryResult>(getAthleteApiResponse)).Returns(getAthleteQueryResult);

            var result = await getAthleteQueryHandler.Handle(new GetAthleteQuery(), CancellationToken.None);

            result.Should().BeEquivalentTo(getAthleteQueryResult);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenApiCallFails()
        {
            _mockApiClient.Setup(api => api.Get<GetAthleteApiResponse>(It.IsAny<GetAthleteApiRequest>())).ThrowsAsync(new Exception("Api call failed"));
            var getAthleteQuery = _fixture.Create<GetAthleteQuery>();

            Func<Task> result = async () => await getAthleteQueryHandler.Handle(getAthleteQuery, CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMappingFails()
        {
            var getAthleteApiResponse = _fixture.Create<GetAthleteApiResponse>();
            _mockApiClient.Setup(x => x.Get<GetAthleteApiResponse>(It.IsAny<GetAthleteApiRequest>())).ReturnsAsync(getAthleteApiResponse);
            _mockMapper.Setup(x => x.Map<GetAthleteQueryResult>(getAthleteApiResponse)).Throws(new Exception("Mapping failed"));
            var getAthleteQuery = _fixture.Create<GetAthleteQuery>();

            Func<Task> result = async() => await getAthleteQueryHandler.Handle(getAthleteQuery, CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }
    }
}
