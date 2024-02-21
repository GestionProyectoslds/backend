
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GDP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

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

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _service.GetActivities();
        }

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
        [HttpGet("user/{id}"), Authorize()]
        public async Task<ActionResult<List<Activity>>> GetActivitiesByUser(int id)
        {
            return await _service.GetActivitiesByUser(id);
        }

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