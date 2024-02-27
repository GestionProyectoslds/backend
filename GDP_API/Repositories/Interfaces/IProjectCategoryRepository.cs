using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IProjectCategoryRepository
{
    Task<IEnumerable<ProjectCategory>> GetProjectCategories();
    Task<ProjectCategory> GetProjectCategoryById(int id);
    Task<IEnumerable<ProjectCategory>> GetProjectCategoriesByFilter(ProjectCategoryFilterDTO projectCategory);
    Task<ProjectCategory> CreateProjectCategory(ProjectCategory projectCategory);
    Task<ProjectCategory> UpdateProjectCategory(ProjectCategory projectCategory);
    Task<ProjectCategory> DeleteProjectCategory(int id);
}
