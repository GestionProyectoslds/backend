
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

        [HttpGet]
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
        [HttpGet("{id}")]
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
        [HttpPut]
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
        [HttpDelete("{id}")]
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