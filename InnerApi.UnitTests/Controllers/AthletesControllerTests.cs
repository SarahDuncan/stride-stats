using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
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

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(athlete, okResult.Value);
        }

        [Fact]
        public async Task GetAthleteStats_ReturnsOkResult_WithAthleteStats()
        {
            var athleteStats = new GetAthletesStatsQueryResult();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthletesStatsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(athleteStats);

            var result = await _controller.GetAthleteStats(It.IsAny<long>());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(athleteStats, okResult.Value);
        }

        [Fact]
        public async Task GetAthlete_ThrowsException_LogsError()
        {
            var exception = new Exception("Test exception");
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAthleteQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var result = await Assert.ThrowsAsync<Exception>(() => _controller.GetAthlete());
            Assert.Equal("An error occurred while retrieving the athlete.", result.Message);
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

            var result = await Assert.ThrowsAsync<Exception>(() => _controller.GetAthleteStats(It.IsAny<long>()));
            Assert.Equal("An error occurred while retrieving the athlete stats.", result.Message);
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
