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

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project> GetProjectById(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        return project ?? throw new KeyNotFoundException(PNF);
    }

    public async Task<Project> CreateProject(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task UpdateProject(Project project)
    {
        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

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
}