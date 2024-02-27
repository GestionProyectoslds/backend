using GDP_API.Models;
using GDP_API.Models.DTOs;

public interface IProjectCategoryService
{
    Task<IEnumerable<ProjectCategory>> GetProjectCategories();
    Task<ProjectCategory> GetProjectCategoryById(int id);
    Task<IEnumerable<ProjectCategory>> GetProjectCategoriesByFilter(ProjectCategoryFilterDTO projectCategoryDTO);
    Task<ProjectCategory> CreateProjectCategory(ProjectCategoryDTO projectCategory);
    Task<ProjectCategory> UpdateProjectCategory(ProjectCategoryDTO projectCategory);
    Task<ProjectCategory> DeleteProjectCategory(int id);
}
