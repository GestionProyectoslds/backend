using GDP_API.Models;
using GDP_API.Data;
using Microsoft.EntityFrameworkCore;
public class ProjectRepository : IProjectRepository
{
    private readonly DataContext _context;
    const string PNF = "Project not found";
    public ProjectRepository(DataContext context)
    {
        _context = context;
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
        if (project is null) throw new KeyNotFoundException();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}