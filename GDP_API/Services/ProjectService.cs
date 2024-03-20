using GDP_API;
using GDP_API.Models;
using GDP_API.Models.DTOs;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IUserService _userService;
    const string InvalidID = "Invalid user or project ID";
    const string NF = "User not found";
    const string PNF = "Project not found";
    const string SWW = "Something went wrong";
    private const string PMNF = "Project manager not found";

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
            int? pmID = null;
            if (projectDto.ProjectManagerId is not null && projectDto.ProjectManagerId != 0)
            {
                var projectManager = await _userService.GetUser(projectDto.ProjectManagerId!.Value);
                if (projectManager is null)
                {
                    throw new KeyNotFoundException(PMNF);
                }
                pmID = projectManager.Id;
            }

            var project = new Project
            {
                Name = projectDto.Name,
                Budget = projectDto.Budget,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                Description = projectDto.Description,
                Comments = projectDto.Comments,
                IsActive = projectDto.IsActive,
                Priority = projectDto.Priority,
                ProjectManagerId = pmID,
            };

            return await _repository.CreateProject(project);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{SWW}\n{PMNF}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
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
            //DRY? never heard of her
            int? pmID = null;
            if (projectDto.ProjectManagerId is not null && projectDto.ProjectManagerId != 0)
            {
                var projectManager = await _userService.GetUser(projectDto.ProjectManagerId!.Value);
                if (projectManager is null)
                {
                    throw new KeyNotFoundException(PMNF);
                }
                pmID = projectManager.Id;
            }
            var project = await _repository.GetProjectById(projectDto.Id);
            if (project is null)
            {
                throw new ArgumentException(PNF);
            }
            project.Name = projectDto.Name;
            project.Budget = projectDto.Budget;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.Description = projectDto.Description;
            project.Comments = projectDto.Comments;
            project.IsActive = projectDto.IsActive;
            project.Priority = projectDto.Priority;
            project.ProjectManagerId = pmID;

            await _repository.UpdateProject(project);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
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
    public async Task<IEnumerable<User>> GetUsersByProject(int id, UserType userType = 0)
    {
        try
        {
            await ValidateProject(id);
            return await _repository.GetUsersByProject(id, userType);
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting users by project", ex);
        }
    }
    public async Task<IEnumerable<Project>> GetProjectsByFilter(ProjectFilterDTO filter)
    {
        try
        {
            return await _repository.GetProjectsByFilter(filter);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(SWW, ex);

        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
        }
    }

    public async Task LinkCategoryProject(int categoryId, int projectId)
    {
        try
        {
            await _repository.LinkCategoryProject(categoryId, projectId);
        }
        catch (Exception ex)
        {
            throw new Exception($"{SWW}\n{ex.Message}", ex);
        }
    }
    public async Task UnLinkCategoryProject(int categoryId, int projectId)
    {
        try
        {
            await _repository.UnLinkCategoryProject(categoryId, projectId);
        }
        catch (Exception ex)
        {
            throw new Exception($"{SWW}\n{ex.Message}", ex);
        }
    }
    #region private methods
    private async Task ValidateProject(int id)
    {
        bool projectExists = await _repository.GetProjectById(id) is not null;
        if (!projectExists)
        {
            throw new KeyNotFoundException(PNF);
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



    #endregion

}