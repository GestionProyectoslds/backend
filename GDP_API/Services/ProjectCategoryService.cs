using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

public class ProjectCategoryService : IProjectCategoryService
{
    private readonly IProjectCategoryRepository _repository;
    private readonly ILogger<ProjectCategoryService> _logger;
    private const string SWW = "Something went wrong";
    private const string CAE = "Category already exists";
    private const string CNF = "Category not found";
    public ProjectCategoryService(IProjectCategoryRepository projectCategoryRepository, ILogger<ProjectCategoryService> logger)
    {
        _repository = projectCategoryRepository;
        _logger = logger;
    }

    public async Task<ProjectCategory> CreateProjectCategory(ProjectCategoryDTO categoryDTO)
    {

        try
        {
            var category = new ProjectCategory
            {
                Name = categoryDTO.Name,
            };
            return await _repository.CreateProjectCategory(category);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"{SWW}\n{CAE}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
        }

    }

    public async Task<IEnumerable<ProjectCategory>> GetProjectCategories()
    {
        try
        {
            return await _repository.GetProjectCategories();
        }
        catch (Exception ex)
        {
            throw new Exception($"{SWW}\n{ex.Message}", ex);
        }
    }
    public async Task<ProjectCategory> GetProjectCategoryById(int id)
    {
        try
        {
            return await _repository.GetProjectCategoryById(id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{SWW}\n{CNF}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);

        }
    }



    public async Task<ProjectCategory> UpdateProjectCategory(ProjectCategoryDTO projectCategory)
    {
        var category = new ProjectCategory
        {
            Id = projectCategory.Id,
            Name = projectCategory.Name,

        };
        try
        {
            return await _repository.UpdateProjectCategory(category);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{SWW}\n{CNF}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
        }
    }

    public Task<ProjectCategory> DeleteProjectCategory(int id)
    {
        try
        {
            return _repository.DeleteProjectCategory(id);
        }
        catch (KeyNotFoundException ex)
        {
            throw new KeyNotFoundException($"{SWW}\n{CNF}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
        }
    }

    public async Task<IEnumerable<ProjectCategory>> GetProjectCategoriesByFilter(ProjectCategoryFilterDTO projectCategoryDTO)
    {
        try
        {
            return await _repository.GetProjectCategoriesByFilter(projectCategoryDTO);
        }
        catch (Exception ex)
        {
            throw new Exception(SWW, ex);
        }
    }
}