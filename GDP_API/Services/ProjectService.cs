using GDP_API.Models;
using GDP_API.Models.DTOs;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Project>> GetAllProjects()
    {
        return _repository.GetAllProjects();
    }

    public Task<Project> GetProjectById(int id)
    {
        try
        {
            return _repository.GetProjectById(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new project based on the provided project DTO.
    /// </summary>
    /// <param name="projectDto">The project DTO containing the project details.</param>
    /// <returns>The created project.</returns>
    public async Task<Project> CreateProject(ProjectCreationDTO projectDto)
    {
        try
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Budget = projectDto.Budget,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate
            };

            return await _repository.CreateProject(project);
        }
        catch (Exception ex)
        {
            throw new Exception("Error creating project", ex);
        }

    }

    /// <summary>
    /// Updates a project with the provided project DTO.
    /// </summary>
    /// <param name="projectDto">The project DTO containing the updated project information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateProject(ProjectDTO projectDto)
    {
        try
        {
            var project = await _repository.GetProjectById(projectDto.Id);
            if (project is null)
            {
                throw new ArgumentException("Project not found");
            }
            project.Name = projectDto.Name;
            project.Budget = projectDto.Budget;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;

            await _repository.UpdateProject(project);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating project", ex);
        }

    }

    public Task DeleteProject(int id)
    {
        return _repository.DeleteProject(id);
    }
}