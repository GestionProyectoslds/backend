using GDP_API.Models;
using GDP_API.Data;
using Microsoft.EntityFrameworkCore;
using GDP_API;
using GDP_API.Models.DTOs;
public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;
    private readonly ILogger<ProjectRepository> _logger;
    const string PNF = "Project not found";
    const string NotLinked = "User is not linked to project";
    const string NoFilter = "At least one filter property must be set";
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
    public async Task<IEnumerable<Project>> GetProjectsByFilter(ProjectFilterDTO filter)
    {
        // Check if all properties of the filter are null
        if (filter.GetType().GetProperties().All(prop => prop.GetValue(filter) == null))
        {
            // If all properties are null, return an empty list
            throw new ArgumentException(NoFilter);
        }

        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(p => p.Name.Contains(filter.Name));
        }

        if (filter.UserId is not null && filter.UserId != 0)
        {
            query = query.Where(p => p.UserHasProjects.Any(up => up.UserId == filter.UserId));
        }

        if (filter.StartDate.HasValue)
        {
            query = query.Where(p => p.StartDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(p => p.EndDate <= filter.EndDate.Value);
        }
        if (filter.MinBudget.HasValue)
        {
            query = query.Where(p => p.Budget >= filter.MinBudget.Value);
        }

        if (filter.MaxBudget.HasValue)
        {
            query = query.Where(p => p.Budget <= filter.MaxBudget.Value);
        }

        if (filter.MinCost.HasValue)
        {
            query = query.Where(p => p.Cost >= filter.MinCost.Value);
        }

        if (filter.MaxCost.HasValue)
        {
            query = query.Where(p => p.Cost <= filter.MaxCost.Value);
        }
        return await query.ToListAsync();
    }


}