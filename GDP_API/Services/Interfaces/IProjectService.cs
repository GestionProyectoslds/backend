using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<Project> CreateProject(ProjectCreationDTO projectDto);
    Task UpdateProject(ProjectDTO projectDto);
    Task DeleteProject(int id);
}