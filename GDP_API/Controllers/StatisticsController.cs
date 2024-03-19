using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Retrieves the statistics.
        /// </summary>
        /// <returns>The statistics as an <see cref="IActionResult"/>.</returns>
        [HttpGet, Authorize()]
        public async Task<IActionResult> GetStatistics()
        {
            var jwt = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            var statistics = await _statisticsService.GetStatistics(jwt);
            if (statistics == null)
            {
                return NotFound(); // Return 404 if there are no statistics
            }

            return Ok(statistics);
        }
    }
}