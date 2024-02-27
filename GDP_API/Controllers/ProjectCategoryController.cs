
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GDP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectCategoryController : ControllerBase
    {
        private readonly IProjectCategoryService _service;
        private readonly ILogger<ProjectCategoryController> _logger;
        public ProjectCategoryController(IProjectCategoryService service, ILogger<ProjectCategoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all project categories.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result that returns the project categories.</returns>
        [HttpGet, Authorize()]
        public async Task<ActionResult<IEnumerable<ProjectCategory>>> GetProjectCategories()
        {
            try
            {
                var categories = await _service.GetProjectCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project categories");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a project category by its ID.
        /// </summary>
        /// <param name="id">The ID of the project category to retrieve.</param>
        /// <returns>An ActionResult containing the project category if found, or an error message if not found.</returns>
        [HttpGet("{id}"), Authorize()]
        public async Task<ActionResult<ProjectCategory>> GetProjectCategoryById(int id)
        {
            try
            {
                var category = await _service.GetProjectCategoryById(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project category");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Filters project categories based on the provided filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria.</param>
        /// <returns>An IActionResult representing the filtered project categories.</returns>
        [HttpPost("filter"), Authorize()]
        public async Task<IActionResult> Filter(ProjectCategoryFilterDTO filter)
        {
            var projectCategories = await _service.GetProjectCategoriesByFilter(filter);

            if (projectCategories == null)
            {
                return NotFound();
            }

            return Ok(projectCategories);
        }
        /// <summary>
        /// Creates a new project category.
        /// </summary>
        /// <param name="projectCategory">The project category to create.</param>
        /// <returns>The created project category.</returns>
        [HttpPost]
        public async Task<ActionResult<ProjectCategory>> CreateProjectCategory(ProjectCategoryDTO projectCategory)
        {
            try
            {
                var category = await _service.CreateProjectCategory(projectCategory);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project category");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Updates a project category.
        /// </summary>
        /// <param name="projectCategory">The project category to update.</param>
        /// <returns>The updated project category.</returns>
        [HttpPut, Authorize()]
        public async Task<ActionResult<ProjectCategory>> UpdateProjectCategory(ProjectCategoryDTO projectCategory)
        {
            try
            {
                var category = await _service.UpdateProjectCategory(projectCategory);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project category");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Deletes a project category by its ID.
        /// </summary>
        /// <param name="id">The ID of the project category to delete.</param>
        /// <returns>An ActionResult containing the deleted project category if successful, or an error message if an exception occurs.</returns>
        [HttpDelete("{id}"), Authorize()]
        public async Task<ActionResult<ProjectCategory>> DeleteProjectCategory(int id)
        {
            try
            {
                var category = await _service.DeleteProjectCategory(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project category");
                return BadRequest(ex.Message);
            }
        }

    }
}