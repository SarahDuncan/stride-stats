using Application.Commands.UpdateAthlete;
using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StrideStats.InnerApi.Controllers;
using System.Net;

namespace InnerApi.UnitTests.Controllers
{
    public class AthletesControllerTests
    {
        private readonly Mock<ILogger<AthleteController>> _mockLogger;
        private readonly Mock<IMediator> _mockMediator;
        private readonly AthleteController _controller;
        private readonly IFixture _fixture;

        public AthletesControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _mockLogger = _fixture.Freeze<Mock<ILogger<AthleteController>>>();
            _mockMediator = _fixture.Freeze<Mock<IMediator>>();
            _controller = new AthleteController(_mockLogger.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task GetAthlete_ReturnsOkResult_WithAthlete()
        {
            var athleteQueryResult = _fixture.Create<GetAthleteQueryResult>();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthleteQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(athleteQueryResult);

            var result = await _controller.GetAthlete();

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.Should().BeEquivalentTo(athleteQueryResult);
        }

        [Fact]
        public async Task GetAthleteStats_ReturnsOkResult_WithAthleteStats()
        {
            var athleteStatsQueryResult = _fixture.Create<GetAthletesStatsQueryResult>();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthletesStatsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(athleteStatsQueryResult);

            var result = await _controller.GetAthleteStats(It.IsAny<long>());

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.Should().BeEquivalentTo(athleteStatsQueryResult);
        }

        [Fact]
        public async Task UpdateAthlete_ReturnsOkResult_WithAthleteData()
        {
            var updateAthleteCommandResult = _fixture.Create<UpdateAthleteCommandResult>();
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateAthleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(updateAthleteCommandResult);

            var result = await _controller.UpdateAthlete(It.IsAny<float>());

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.Should().BeEquivalentTo(updateAthleteCommandResult);
        }

        [Fact]
        public async Task GetAthlete_ThrowsException_LogsError()
        {
            var exception = _fixture.Create<Exception>();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthleteQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            Func<Task> result = _controller.GetAthlete;

            var resultException = await result.Should().ThrowAsync<Exception>();
            resultException.Which.Message.Should().Be("An error occurred while retrieving the athlete.", exception.Message);
            _mockLogger.Verify(
                   x => x.Log(
                       It.Is<LogLevel>(l => l == LogLevel.Error),
                       It.IsAny<EventId>(),
                       It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while retrieving the athlete")),
                       It.IsAny<Exception>(),
                       It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                   Times.Once);
        }

        [Fact]
        public async Task GetAthleteStats_ThrowsException_LogsError()
        {
            var exception = _fixture.Create<Exception>();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthletesStatsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            Func<Task> result = () => _controller.GetAthleteStats(It.IsAny<long>());

            var resultException = await result.Should().ThrowAsync<Exception>();
            resultException.Which.Message.Should().Be("An error occurred while retrieving the athlete stats.", exception.Message);
            _mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Error),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while retrieving the athlete stats.")),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
               Times.Once);
        }

        [Fact]
        public async Task UpdateAthlete_ThrowsException_LogsError()
        {
            var exception = _fixture.Create<Exception>();
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateAthleteCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            Func<Task> result = () => _controller.UpdateAthlete(It.IsAny<float>());

            var resultException = await result.Should().ThrowAsync<Exception>();
            resultException.Which.Message.Should().Be("An error occurred while updating the athlete.", exception.Message);
            _mockLogger.Verify(
                   x => x.Log(
                       It.Is<LogLevel>(l => l == LogLevel.Error),
                       It.IsAny<EventId>(),
                       It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while updating the athlete.")),
                       It.IsAny<Exception>(),
                       It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                   Times.Once);
        }
    }
}
