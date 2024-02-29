
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GDP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activityDto">The activity data transfer object.</param>
        /// <returns>The created activity.</returns>
        [HttpPost("create"), Authorize()]
        public async Task<ActionResult<Activity>> CreateActivity(ActivityDto activityDto)
        {
            try
            {
                var activity = await _service.CreateActivity(activityDto);
                return Ok(activity.Description);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Retrieves all activities.
        /// </summary>
        /// <returns>A list of activities.</returns>
        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _service.GetActivities();
        }

        /// <summary>
        /// Retrieves an activity by its ID.
        /// </summary>
        /// <param name="id">The ID of the activity to retrieve.</param>
        /// <returns>An ActionResult containing the activity if found, or NotFound if not found.</returns>
        [HttpGet("{id}"), Authorize()]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var activity = await _service.GetActivity(id);
            if (activity is null)
            {
                return NotFound();
            }
            return activity;
        }
        /// <summary>
        /// Retrieves a list of activities associated with a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of activities.</returns>
        [HttpGet("user/{id}"), Authorize()]
        public async Task<ActionResult<List<Activity>>> GetActivitiesByUser(int id)
        {
            return await _service.GetActivitiesByUser(id);
        }
        /// <summary>
        /// Filters activities based on the provided filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria.</param>
        /// <returns>A list of filtered activities.</returns>
        [HttpPost("filter"), Authorize()]
        public async Task<ActionResult<List<Activity>>> FilterActivities(ActivityFilterDto filter)
        {
            try
            {
                return await _service.FilterActivities(filter);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Updates an activity with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the activity to update.</param>
        /// <param name="activityDto">The updated activity data.</param>
        /// <returns>An IActionResult representing the result of the update operation.</returns>
        [HttpPut("{id}"), Authorize()]
        public async Task<IActionResult> UpdateActivity(int id, ActivityDto activityDto)
        {

            try
            {
                await _service.UpdateActivity(id, activityDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Deletes an activity by its ID.
        /// </summary>
        /// <param name="id">The ID of the activity to delete.</param>
        /// <returns>An IActionResult representing the result of the delete operation.</returns>
        [HttpDelete("{id}"), Authorize()]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                await _service.DeleteActivity(id);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return Ok();
        }
        /// <summary>
        /// Links a user to an activity.
        /// </summary>
        /// <param name="linkActivityDto">The DTO containing the activity ID and user ID.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost("assignUser"), Authorize()]
        public async Task<IActionResult> LinkUser(LinkActivityDto linkActivityDto)
        {
            try
            {
                await _service.LinkUserToActivity(linkActivityDto.ActivityId, linkActivityDto.UserId);
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Removes the link between a user and an activity.
        /// </summary>
        /// <param name="linkActivityDto">The DTO containing the activity ID and user ID.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost("removeUser"), Authorize()]
        public async Task<IActionResult> UnlinkUser(LinkActivityDto linkActivityDto)
        {
            try
            {
                await _service.UnlinkUserFromActivity(linkActivityDto.ActivityId, linkActivityDto.UserId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}