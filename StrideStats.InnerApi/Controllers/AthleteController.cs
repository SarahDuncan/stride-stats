using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StrideStats.InnerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AthleteController : ControllerBase
    {
        private readonly ILogger<AthleteController> _logger;
        private readonly IMediator _mediator;

        public AthleteController(ILogger<AthleteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAthlete()
        {
            try
            {
                var result = await _mediator.Send(new GetAthleteQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the athlete.");
                throw new Exception($"An error occurred while retrieving the athlete. Exception: {ex.Message}", ex);
            }
        }

        [HttpGet("{athleteId}/stats")]
        public async Task<IActionResult> GetAthleteStats(long athleteId)
        {
            try
            {
                var result = await _mediator.Send(new GetAthletesStatsQuery { AthleteId = athleteId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the athlete stats.");
                throw new Exception($"An error occurred while retrieving the athlete stats. Exception: {ex.Message}", ex);
            }
        }
    }
}
