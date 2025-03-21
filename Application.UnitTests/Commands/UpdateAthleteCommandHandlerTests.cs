using Application.Commands.UpdateAthlete;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Interfaces.Api;
using Domain.Requests;
using Domain.Responses;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Commands
{
    public class UpdateAthleteCommandHandlerTests
    {
        private readonly Mock<IApiClient> _mockApiClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateAthleteCommandHandler updateAthleteCommandHandler;
        private readonly IFixture _fixture;

        public UpdateAthleteCommandHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockApiClient = _fixture.Freeze<Mock<IApiClient>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();
            updateAthleteCommandHandler = new UpdateAthleteCommandHandler(_mockApiClient.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ReturnsUpdateAthleteCommandResult()
        {
            var weight = _fixture.Create<float>();
            var updateAthleteApiResponse = _fixture.Create<UpdateAthleteApiResponse>();
            var updateAthleteCommandResult = _fixture.Create<UpdateAthleteCommandResult>();
            _mockApiClient.Setup(x => x.Put<UpdateAthleteApiResponse>(It.IsAny<UpdateAthleteApiRequest>())).ReturnsAsync(updateAthleteApiResponse);
            _mockMapper.Setup(x => x.Map<UpdateAthleteCommandResult>(updateAthleteApiResponse)).Returns(updateAthleteCommandResult);

            var result = await updateAthleteCommandHandler.Handle(new UpdateAthleteCommand(weight), CancellationToken.None);

            result.Should().BeEquivalentTo(updateAthleteCommandResult);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenApiCallFails()
        {
            _mockApiClient.Setup(api => api.Put<UpdateAthleteApiResponse>(It.IsAny<UpdateAthleteApiRequest>())).ThrowsAsync(new Exception("Api call failed"));
            var updateAthleteCommand = _fixture.Create<UpdateAthleteCommand>();

            Func<Task> result = async () => await updateAthleteCommandHandler.Handle(updateAthleteCommand, CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMappingFails()
        {
            var weight = _fixture.Create<float>();
            var updateAthleteApiResponse = _fixture.Create<UpdateAthleteApiResponse>();
            _mockApiClient.Setup(x => x.Put<UpdateAthleteApiResponse>(It.IsAny<UpdateAthleteApiRequest>())).ReturnsAsync(updateAthleteApiResponse);
            _mockMapper.Setup(x => x.Map<UpdateAthleteCommandResult>(updateAthleteApiResponse)).Throws(new Exception("Mapping failed"));

            Func<Task> result = async() => await updateAthleteCommandHandler.Handle(new UpdateAthleteCommand(weight), CancellationToken.None);

            await result.Should().ThrowAsync<Exception>();
        }
    }
}
