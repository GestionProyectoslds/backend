using GDP_API.Data;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
public class ProjectCategoryRepository : IProjectCategoryRepository
{
    private const string ECPC = "Error creating project category";
    private const string PCNF = "Project category not found";
    private readonly DataContext _context;
    private readonly ILogger<ProjectCategoryRepository> _logger;
    public ProjectCategoryRepository(DataContext context, ILogger<ProjectCategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ProjectCategory> CreateProjectCategory(ProjectCategory projectCategory)
    {
        try
        {
            await _context.ProjectCategories.AddAsync(projectCategory);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{ECPC}\n{e.Message}");
            throw new DbUpdateException(ECPC);
        }
        await _context.SaveChangesAsync();
        return projectCategory;
    }
    public async Task<ProjectCategory> DeleteProjectCategory(int id)
    {
        var projectCategory = await _context.ProjectCategories.FindAsync(id);
        if (projectCategory == null)
        {
            throw new KeyNotFoundException(PCNF);
        }
        _context.ProjectCategories.Remove(projectCategory);
        await _context.SaveChangesAsync();
        return projectCategory;
    }
    public async Task<IEnumerable<ProjectCategory>> GetProjectCategories()
    {
        return await _context.ProjectCategories.ToListAsync();
    }

    public async Task<IEnumerable<ProjectCategory>> GetProjectCategoriesByFilter(ProjectCategoryFilterDTO projectCategory)
    {
        var query = _context.ProjectCategories.AsQueryable();
        if (!string.IsNullOrEmpty(projectCategory.Name))
        {
            query = query.Where(pc => pc.Name.Contains(projectCategory.Name));
        }
        return await query.ToListAsync();
    }

    public async Task<ProjectCategory> GetProjectCategoryById(int id)
    {
        return await _context.ProjectCategories.FindAsync(id) ?? throw new KeyNotFoundException(PCNF);
    }
    public async Task<ProjectCategory> UpdateProjectCategory(ProjectCategory projectCategory)
    {
        if (!_context.ProjectCategories.Any(pc => pc.Id == projectCategory.Id))
        {
            throw new KeyNotFoundException(PCNF);
        }
        _context.ProjectCategories.Update(projectCategory);
        await _context.SaveChangesAsync();
        return projectCategory;
    }
}