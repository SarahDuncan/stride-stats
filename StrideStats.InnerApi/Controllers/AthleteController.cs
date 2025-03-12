using Application.Queries.GetAthlete;
using Application.Queries.GetAthletesStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StrideStats.InnerApi.Controllers
{
    /// <summary>
    /// Controller for handling athlete-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AthleteController : ControllerBase
    {
        private readonly ILogger<AthleteController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthleteController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        /// <param name="mediator">The mediator instance for sending queries and commands.</param>
        public AthleteController(ILogger<AthleteController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets an authorised athlete's information.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the athlete's information if successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs while getting the athlete's information.
        /// </exception>
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

        /// <summary>
        /// Gets the statistics for a specific athlete.
        /// </summary>
        /// <param name="athleteId">The unique identifier of the athlete whose stats are to be retrieved.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the athlete's statistics if successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when an error occurs while retrieving the athlete's statistics.
        /// </exception>
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
