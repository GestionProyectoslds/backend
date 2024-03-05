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

        [HttpGet, Authorize()]
        public IActionResult GetStatistics()
        {
            var statistics = _statisticsService.GetStatistics();
            if (statistics == null)
            {
                return NotFound(); // Return 404 if there are no statistics
            }

            return Ok(statistics);
        }
    }
}