using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<IEnumerable<User>> GetUsersByProject(int id, GDP_API.UserType userType = 0);
    Task<IEnumerable<Project>> GetProjectsByFilter(ProjectFilterDTO filter);
    Task<Project> CreateProject(ProjectCreationDTO projectDto);
    Task UpdateProject(ProjectDTO projectDto);
    Task DeleteProject(int id);
    Task LinkUserProject(UserProjectLinkDto userProjectLinkDto);
    Task UnlinkUserProject(UserProjectLinkDto userProjectLinkDto);
}