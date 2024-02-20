using GDP_API.Models;
using GDP_API.Models.DTOs;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IUserService _userService;
    const string InvalidID = "Invalid user or project ID";
    const string NF = "User not found";
    const string PNF = "Project not found";
    public ProjectService(IProjectRepository repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await _repository.GetAllProjects();
    }

    public async Task<Project> GetProjectById(int id)
    {
        try
        {
            return await _repository.GetProjectById(id);
        }
        catch (Exception ex)
        {
            if (ex is KeyNotFoundException)
            {
                throw new KeyNotFoundException(PNF);
            }
            throw new Exception("Error getting project", ex);
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
                EndDate = projectDto.EndDate,
                Description = projectDto.Description,
                Comments = projectDto.Comments
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
    public async Task LinkUserProject(UserProjectLinkDto userProjectLinkDto)
    {
        try
        {
            await linkingValidations(userProjectLinkDto);
            if (await _repository.UserHasProject(userProjectLinkDto.UserId, userProjectLinkDto.ProjectId))
            {
                throw new ArgumentException("User is already linked to project");
            }
            await _repository.LinkUserProject(userProjectLinkDto.UserId, userProjectLinkDto.ProjectId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error linking user to project:\n {ex.Message}");
        }
    }
    public async Task UnlinkUserProject(UserProjectLinkDto userProjectLinkDto)
    {
        try
        {
            await linkingValidations(userProjectLinkDto);

            await _repository.UnlinkUserProject(userProjectLinkDto.UserId, userProjectLinkDto.ProjectId);

        }
        catch (Exception ex)
        {
            throw new Exception($"Error unlinking user to project:\n {ex.Message}");
        }
    }
    private async Task linkingValidations(UserProjectLinkDto userProjectLinkDto)
    {
        if (userProjectLinkDto.UserId == 0 || userProjectLinkDto.ProjectId == 0)
        {
            throw new ArgumentException(InvalidID);
        }
        await ValidateUser(userProjectLinkDto.UserId);
        await ValidateProject(userProjectLinkDto.ProjectId);

    }
    private async Task ValidateUser(int userId)
    {
        bool userExists = await _userService.GetUser(userId) is not null;
        if (!userExists)
        {
            throw new KeyNotFoundException(NF);
        }
    }

    private async Task ValidateProject(int id)
    {
        bool projectExists = await _repository.GetProjectById(id) is not null;
        if (!projectExists)
        {
            throw new KeyNotFoundException(PNF);
        }
    }
}