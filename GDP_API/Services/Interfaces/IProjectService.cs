using GDP_API.Models;
public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<Project> CreateProject(Project project);
    Task UpdateProject(Project project);
    Task DeleteProject(int id);
}