using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
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

        public AthletesControllerTests()
        {
            _mockLogger = new Mock<ILogger<AthleteController>>();
            _mockMediator = new Mock<IMediator>();
            _controller = new AthleteController(_mockLogger.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task GetAthlete_ReturnsOkResult_WithAthlete()
        {
            var athlete = new GetAthleteQueryResult();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthleteQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(athlete);

            var result = await _controller.GetAthlete();

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.Should().BeEquivalentTo(athlete);
        }

        [Fact]
        public async Task GetAthleteStats_ReturnsOkResult_WithAthleteStats()
        {
            var athleteStats = new GetAthletesStatsQueryResult();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthletesStatsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(athleteStats);

            var result = await _controller.GetAthleteStats(It.IsAny<long>());

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okResult.Value.Should().BeEquivalentTo(athleteStats);
        }

        [Fact]
        public async Task GetAthlete_ThrowsException_LogsError()
        {
            var exception = new Exception("Test exception");
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthleteQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Func<Task> result = _controller.GetAthlete;

            var resultException = await result.Should().ThrowAsync<Exception>();
            resultException.Which.Message.Should().Be("An error occurred while retrieving the athlete.", resultException.Which.Message);
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
            var exception = new Exception("Test exception");
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthletesStatsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            Func<Task> result = () => _controller.GetAthleteStats(It.IsAny<long>());

            var resultException = await result.Should().ThrowAsync<Exception>();
            resultException.Which.Message.Should().Be("An error occurred while retrieving the athlete stats.", resultException.Which.Message);
            _mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Error),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while retrieving the athlete stats.")),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
               Times.Once);
        }
    }
}
