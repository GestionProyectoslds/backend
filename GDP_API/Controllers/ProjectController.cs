
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _service.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{id}"), Authorize()]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _service.GetProjectById(id);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpGet("{id}/users"), Authorize()]
        public async Task<IActionResult> GetUsersByProject(int id, UserType userType = 0)
        {
            try
            {
                var users = await _service.GetUsersByProject(id, userType);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("user/id/{id}"), Authorize()]
        public async Task<IActionResult> GetProjectsByUserId(int id)
        {
            try
            {
                var projects = await _service.GetProjectsByUserId(id);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpGet("user/{identifier}"), Authorize()]
        public async Task<IActionResult> GetProjectsByUserName(string identifier, UserSearchMethod searchMethod)
        {
            try
            {
                switch (searchMethod)
                {
                    case UserSearchMethod.Email:
                        return Ok(await _service.GetProjectsByUserEmail(identifier));
                    case UserSearchMethod.Name:
                        return Ok(await _service.GetProjectsByUserName(identifier));
                    default:
                        return BadRequest("Invalid search method");
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create"), Authorize()]
        public async Task<IActionResult> CreateProject(ProjectCreationDTO projectDto)
        {
            var project = await _service.CreateProject(projectDto);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("update"), Authorize()]
        public async Task<IActionResult> UpdateProject(ProjectDTO projectDto)
        {
            try
            {
                await _service.UpdateProject(projectDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}"), Authorize()]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _service.DeleteProject(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("addUser")]
        public async Task<IActionResult> LinkUserProject(UserProjectLinkDto userProjectLinkDto)
        {
            try
            {
                await _service.LinkUserProject(userProjectLinkDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("removeUser")]
        public async Task<IActionResult> UnlinkUserProject(UserProjectLinkDto userProjectLinkDto)
        {
            try
            {
                await _service.UnlinkUserProject(userProjectLinkDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}