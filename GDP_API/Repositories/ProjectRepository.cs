using GDP_API.Models;
using GDP_API.Data;
using Microsoft.EntityFrameworkCore;
using GDP_API;
public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;
    private readonly ILogger<ProjectRepository> _logger;
    const string PNF = "Project not found";
    const string NotLinked = "User is not linked to project";
    public ProjectRepository(DataContext context, ILogger<ProjectRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all projects from the database.
    /// </summary>
    /// <returns>An enumerable collection of projects.</returns>
    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    /// <summary>
    /// Retrieves a project by its ID.
    /// </summary>
    /// <param name="id">The ID of the project to retrieve.</param>
    /// <returns>The project with the specified ID, or throws a KeyNotFoundException if not found.</returns>
    public async Task<Project> GetProjectById(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        return project ?? throw new KeyNotFoundException(PNF);
    }

    /// <summary>
    /// Creates a new project in the database.
    /// </summary>
    /// <param name="project">The project to be created.</param>
    /// <returns>The created project.</returns>
    public async Task<Project> CreateProject(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    /// <summary>
    /// Updates a project in the database.
    /// </summary>
    /// <param name="project">The project to be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateProject(Project project)
    {
        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a project by its ID.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeleteProject(int id)
    {

        var project = await _context.Projects.FindAsync(id);
        if (project is null)
        {
            throw new KeyNotFoundException(PNF);
        }
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Links a user to a project by creating a new UserHasProject record in the database.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="projectId">The ID of the project.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LinkUserProject(int userId, int projectId)
    {
        var userHasProject = new UserHasProject
        {
            UserId = userId,
            ProjectId = projectId
        };

        await _context.UserHasProjects.AddAsync(userHasProject);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Unlinks a user from a project asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="projectId">The ID of the project.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UnlinkUserProject(int userId, int projectId)
    {
        _logger.LogInformation($"Unlinking user {userId} from project {projectId}");
        var userHasProject = new UserHasProject
        {
            UserId = userId,
            ProjectId = projectId
        };
        if (!await _context.UserHasProjects.AnyAsync(x => x.UserId == userId && x.ProjectId == projectId))
        {
            throw new KeyNotFoundException(NotLinked);
        }
        _context.UserHasProjects.Remove(userHasProject);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> UserHasProject(int userId, int projectId)
    {
        return await _context.UserHasProjects.AnyAsync(x => x.UserId == userId && x.ProjectId == projectId);
    }

    /// <summary>
    /// Retrieves a collection of users associated with a specific project.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="userType">The type of user to filter by. Defaults to 0 (all user types).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of users.</returns>
    public async Task<IEnumerable<User>> GetUsersByProject(int projectId, UserType userType = 0)
    {
        return await _context.UserHasProjects
        .Where(up => up.ProjectId == projectId && (userType == 0 || up.User.UserTypeId == userType))
        .Select(up => up.User)
        .ToListAsync();
    }
    #region get projects by user
    /// <summary>
    /// Retrieves a collection of projects associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of projects.</returns>
    public async Task<IEnumerable<Project>> GetProjectsByUserId(int userId)
    {
        return await _context.UserHasProjects
        .Where(up => up.UserId == userId)
        .Select(up => up.Project)
        .ToListAsync();
    }

    /// <summary>
    /// Retrieves a collection of projects associated with a given user name.
    /// </summary>
    /// <param name="userName">The user name to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of projects.</returns>
    public async Task<IEnumerable<Project>> GetProjectsByUserName(string userName)
    {
        return await _context.UserHasProjects
        .Where(up => up.User.Name.Contains(userName))
        .OrderBy(up => up.User.Name != userName)
        .Select(up => up.Project)
        .ToListAsync();
    }

    /// <summary>
    /// Retrieves a collection of projects associated with a user's email.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of projects.</returns>
    public async Task<IEnumerable<Project>> GetProjectsByUserEmail(string email)
    {
        return await _context.UserHasProjects
       .Where(up => up.User.Email == email)
       .Select(up => up.Project)
       .ToListAsync();
    }
    #endregion

}