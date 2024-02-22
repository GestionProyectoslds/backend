using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllProjects();
    Task<Project> GetProjectById(int id);
    Task<IEnumerable<User>> GetUsersByProject(int id, GDP_API.UserType userType = 0);
    Task<IEnumerable<Project>> GetProjectsByUserId(int id);
    Task<IEnumerable<Project>> GetProjectsByUserName(string userName);
    Task<IEnumerable<Project>> GetProjectsByUserEmail(string email);
    Task<Project> CreateProject(ProjectCreationDTO projectDto);
    Task UpdateProject(ProjectDTO projectDto);
    Task DeleteProject(int id);
    Task LinkUserProject(UserProjectLinkDto userProjectLinkDto);
    Task UnlinkUserProject(UserProjectLinkDto userProjectLinkDto);
}