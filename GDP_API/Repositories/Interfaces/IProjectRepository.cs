using GDP_API.Models;
public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<Project> CreateProject(Project project);
    Task UpdateProject(Project project);
    Task DeleteProject(int id);
    Task LinkUserProject(int userId, int projectId);
    Task UnlinkUserProject(int userId, int projectId);
    Task<bool> UserHasProject(int userID, int projectID);
}