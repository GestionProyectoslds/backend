
using GDP_API.Models;
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

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>An IActionResult containing the list of projects.</returns>
        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _service.GetAllProjects();
            return Ok(projects);
        }

        /// <summary>
        /// Retrieves a project by its ID.
        /// </summary>
        /// <param name="id">The ID of the project.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves projects based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the projects.</param>
        /// <returns>An IActionResult containing the projects that match the filter.</returns>
        [HttpPost("filterProjects"), Authorize()]
        public async Task<IActionResult> GetProjectsByFilter(ProjectFilterDTO filter)
        {
            try
            {
                var projects = await _service.GetProjectsByFilter(filter);
                return Ok(projects);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The project data transfer object.</param>
        /// <returns>The created project.</returns>
        [HttpPost("create"), Authorize()]
        public async Task<IActionResult> CreateProject(ProjectCreationDTO projectDto)
        {
            var project = await _service.CreateProject(projectDto);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Updates a project.
        /// </summary>
        /// <param name="projectDto">The project DTO.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
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

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>An IActionResult representing the result of the deletion operation.</returns>
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
        /// <summary>
        /// Links a user to a project.
        /// </summary>
        /// <param name="userProjectLinkDto">The user project link DTO.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        #region LinkAndUnlinkUser
        [HttpPost("addUser"), Authorize()]
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

        /// <summary>
        /// Unlinks a user from a project.
        /// </summary>
        /// <param name="userProjectLinkDto">The user-project link DTO.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        [HttpPost("removeUser"), Authorize()]
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
        #endregion
        #region Categories
        [HttpPost("category/create"), Authorize()]
        public async Task<ActionResult<ProjectCategory>> CreateCategory(ProjectCategoryCreationDTO category)
        {
            try
            {
                var newCategory = await _service.CreateCategory(category);
                return Ok(newCategory);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("category/link")]
        public async Task<ActionResult> LinkCategoryToProject(LinkProjectCategoryDTO linkProjectCategoryDTO)
        {
            try
            {
                await _service.LinkCategoryProject(linkProjectCategoryDTO.CategoryId, linkProjectCategoryDTO.ProjectId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



        [HttpPost("category/unlink")]
        public async Task<ActionResult> UnlinkCategoryFromProject(LinkProjectCategoryDTO linkProjectCategoryDTO)
        {
            try
            {
                await _service.UnLinkCategoryProject(linkProjectCategoryDTO.CategoryId, linkProjectCategoryDTO.ProjectId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}